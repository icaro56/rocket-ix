using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLRocketAttribute
{
	public string Name;
	public int Price;
	public float Value;
}

[XmlRoot("RocketContainer")]
public class XMLRocketAttributes  
{
	[XmlArray("Speed"),XmlArrayItem("Data")]
	public List<XMLRocketAttribute> Speed = new List<XMLRocketAttribute>();

	[XmlArray("Handling"),XmlArrayItem("Data")]
	public List<XMLRocketAttribute> Handling = new List<XMLRocketAttribute>();

	[XmlArray("Rate"),XmlArrayItem("Data")]
	public List<XMLRocketAttribute> Rate = new List<XMLRocketAttribute>();

	[XmlArray("Life"),XmlArrayItem("Data")]
	public List<XMLRocketAttribute> Life = new List<XMLRocketAttribute>();

	[XmlArray("Magnetic"),XmlArrayItem("Data")]
	public List<XMLRocketAttribute> Magnetic = new List<XMLRocketAttribute>();

	public static XMLRocketAttributes Load(string path)
	{
		var serializer = new XmlSerializer(typeof(XMLRocketAttributes));
		var stream = new FileStream(path, FileMode.Open);
		var container = serializer.Deserialize(stream) as XMLRocketAttributes;
		stream.Close();
		return container;
	}

	public static XMLRocketAttributes LoadFromText(string text)
	{
		var serializer = new XmlSerializer(typeof(XMLRocketAttributes));
		return serializer.Deserialize(new StringReader(text)) as XMLRocketAttributes;
	}
}
