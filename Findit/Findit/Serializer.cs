using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Findit;

public class Serializer
{
    public Serializer()
    {
    }

    public void SerializeObject(string filename, SearchParameters objectToSerialize)
    {
        Stream stream = File.Open(filename, FileMode.Create);
        BinaryFormatter bFormatter = new BinaryFormatter();
        bFormatter.Serialize(stream, objectToSerialize);
        stream.Close();
    }

    public SearchParameters DeSerializeObject(string filename)
    {
        SearchParameters objectToSerialize;
        Stream stream = File.Open(filename, FileMode.Open);
        BinaryFormatter bFormatter = new BinaryFormatter();
        objectToSerialize = (SearchParameters)bFormatter.Deserialize(stream);
        stream.Close();
        return objectToSerialize;
    }
}