using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class DalTest
    {
        private TransactionScope tran;      //<-- used to begin a transaction during initialize and rollback during cleanup
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=npcampground;Integrated Security=true";

        [TestInitialize]
        public void Initialize()
        {
            // Initialize a new transaction scope. This automatically begins the transaction.
            tran = new TransactionScope();

            // Open a SqlConnection object using the active transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                //Insert a Dummy Record for Department                
                cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('TestPark', 'Ohio', '2019-01-01', 10, 100, 'test park');", conn);
                cmd.ExecuteNonQuery();

            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
        }


        [TestMethod]
        public void GetParksTest()
        {
            ////Arrange
            //Dal dal = new Dal(connectionString);

            //Park test = new Park();
            //test.Name = "Test";


            ////Act
            //int result = department.CreateDepartment(test);
            //List<Department> testList = department.GetDepartmentsById(result);
            //List<Department> testAllDept = department.GetDepartments();

            //Department testTwo = new Department();
            //testTwo.Name = "TestTwo";
            //testTwo.Id = 3;

            //bool name = department.UpdateDepartment(testTwo);

            ////Assert
            //Assert.AreEqual(result, testList[0].Id);
            //Assert.AreEqual("Test", testList[0].Name);
            //Assert.AreEqual(8, testAllDept.Count);
            //Assert.AreEqual(true, name);
        }
    }
}
