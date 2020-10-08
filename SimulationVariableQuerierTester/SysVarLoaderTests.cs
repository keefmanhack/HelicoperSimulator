using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulationVariableQuerier;
using System.Collections;
using System.IO;

namespace SimulationVariableQuerierTester
{
    [TestClass]
    public class SysVarLoaderTests
    {
        [TestMethod]
        public void canVerifyFile()
        {
            SysVarLoaderImplementation loader = new SysVarLoaderImplementation();
            string testFileLoc = @"testFile.txt";
            loader.setTxtFileLocation(testFileLoc);

            Assert.AreEqual(testFileLoc, loader.getTxtFileLocation());
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void throwsExceptionNoneExistingFile()
        {
            SysVarLoaderImplementation loader = new SysVarLoaderImplementation();
            string testFileLoc = @"fakepath.txt";
            loader.setTxtFileLocation(testFileLoc);
        }


        [TestMethod]
        public void canRetreiveSystemVariablesFromTextFile()
        {
            SysVarLoaderImplementation loader = new SysVarLoaderImplementation();
            string testFileLoc = @"hasVars.txt";
            loader.setTxtFileLocation(testFileLoc);
            loader.loadSysVariables();

            Hashtable t = loader.getVarTable();
            Hashtable test = new Hashtable();
            test.Add("AUTOPILOT PITCH HOLD", null);
            test.Add("STRUCT AMBIENT WIND", null);
            test.Add("LAUNCHBAR POSITION", null);
            test.Add("NUMBER OF CATAPULTS", null);


            CollectionAssert.AreEqual(test.Keys, t.Keys, message: "Keys not equal");
        }

    }
}
