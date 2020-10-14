using System;
using System.Collections;
using System.IO;

interface SysVarLoaderInterface
{
    void loadSysVariables();
    List<object> getVarsAndUnits();
    void setTxtFileLocation(String location);
    String getTxtFileLocation();
}


public class SysVarLoaderImplementation: SysVarLoaderInterface
{
    private Hashtable table;
    private String txtFile;

	public SysVarLoaderImplementation()
	{
        table = new Hashtable();
	}

    public void setTxtFileLocation(String location)
    {
        if (!File.Exists(location))
        {
            throw new FileNotFoundException("Data file is missing");
        }
        txtFile = location;
    }

    public String getTxtFileLocation()
    {
        return this.txtFile;
    }

    public void loadSysVariables()
    {
        StreamReader sr = new StreamReader(this.txtFile);
        String line;

        while((line = sr.ReadLine()) != null)
        {
            table.Add(line, null);
        }
        sr.Close();
    }

    public List<object> getVarsAndUnits()
    {
        List<object> ret = new List<>();

        for(int i =0; i<table.Keys.length; i++){
            ret.add({var: table.Keys[i], unit: "meters per second"});
        }

        return ret;
    }
}
