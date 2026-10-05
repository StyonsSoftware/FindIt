/*
Reads and writes ".fit" saved search files.

This used to use BinaryFormatter.  BinaryFormatter builds whatever types the *file* names
and runs their deserialization callbacks, so a .fit file could execute arbitrary code as
the user who opened it.  The application is built to open a .fit by double-clicking one
(see frmMain_Load), which made that a straightforward way to attack anyone running FindIt.

XmlSerializer typed to SavedSearch can only ever produce a SavedSearch, whatever the file
says, so a hostile file is inert.  DTD handling is turned off as well, which closes the
external-entity and entity-expansion tricks that XML brings with it.

Files written by the old version are no longer readable, by design - reading one is the
exact thing we removed.  IsLegacySearchFile only sniffs a few bytes so callers can say so
plainly instead of showing a parse error.
//*/
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Findit;

public class Serializer
{
    //a BinaryFormatter stream always opens with a serialization header record:
    //record type 0, then the root id as a little-endian int32
    private static readonly byte[] c_LegacyBinaryHeader = { 0x00, 0x01, 0x00, 0x00, 0x00 };

    public Serializer()
    {
    }

    public void SerializeObject(string filename, SearchParameters objectToSerialize)
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;

        using (XmlWriter writer = XmlWriter.Create(filename, settings))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SavedSearch));
            serializer.Serialize(writer, SavedSearch.From(objectToSerialize));
        }
    }

    public SearchParameters DeSerializeObject(string filename)
    {
        using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (XmlReader reader = XmlReader.Create(stream, SafeReaderSettings()))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SavedSearch));
            SavedSearch saved = (SavedSearch)serializer.Deserialize(reader);
            return saved.ToSearchParameters();
        }
    }

    private static XmlReaderSettings SafeReaderSettings()
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Prohibit;  //no DTD == no entity expansion, no external entities
        settings.XmlResolver = null;                      //never go looking for anything the file points at
        settings.IgnoreComments = true;
        settings.IgnoreProcessingInstructions = true;
        settings.IgnoreWhitespace = true;
        return settings;
    }

    public static Boolean IsLegacySearchFile(string filename)
    {
        //this only ever *reads bytes*.  it must never deserialize anything.
        try
        {
            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] header = new byte[c_LegacyBinaryHeader.Length];
                int bytesRead = stream.Read(header, 0, header.Length);
                if (bytesRead < header.Length)
                {
                    return false;
                }
                for (int i = 0; i < header.Length; ++i)
                {
                    if (header[i] != c_LegacyBinaryHeader[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }
}
