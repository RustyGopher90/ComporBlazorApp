using System.Data;

using System.Data.SqlClient;

using System.Reflection.PortableExecutable;

using System.Reflection;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;



namespace Compor_App.Data

{

    public class ComporServices

    {

        private readonly ILogger _logger;



        public ComporServices(ILogger<ComporServices> logger)

        {

            _logger = logger;

        }





        //Connection String

        private string SqlConnectionString = "SomeConnectionString";



        //test table

        private string Table = "ComporTableTest";

        //production table

        //private string Table = "ComporTable";



        //testing off

        //private int Testing = 0;

        //tesing on

        private int Testing = 1;





        internal Task<DataTable> GetAllCompers(DataTable dt)

        {

            BuildAllComperTable(dt);



            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = $"Select EmployeeID, ComporID, FirstName, LastName, ComporLevel, PercentOverage, DollarOverage, Department, Enabled, DateDisabled FROM SomeDataWarehouse.SomeSchema.{Table} order by ComporID";

                    cmd.Connection = conn;

                    try

                    {

                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {

                                dt.Rows.Add(

                                    reader.GetValue(0),

                                    reader.GetValue(1),

                                    reader.GetValue(2),

                                    reader.GetValue(3),

                                    reader.GetValue(4),

                                    reader.GetValue(5),

                                    reader.GetValue(6),

                                    reader.GetValue(7),

                                    reader.GetValue(8),

                                    reader.GetValue(9)

                                    );

                            }

                        }

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error getting all compors : {ex.Message}");

                        throw;

                    }

                }

            }



            return Task.FromResult(dt);

        }



        private void BuildAllComperTable(DataTable dt)

        {

            dt.Columns.Add("EmployeeID", typeof(string));

            dt.Columns.Add("ComporID", typeof(string));

            dt.Columns.Add("FirstName", typeof(string));

            dt.Columns.Add("LastName", typeof(string));

            dt.Columns.Add("ComporLevel", typeof(string));

            dt.Columns.Add("PercentOverage", typeof(decimal));

            dt.Columns.Add("DollarOverage", typeof(decimal));

            dt.Columns.Add("Department", typeof(string));

            dt.Columns.Add("Enabled", typeof(string));

            dt.Columns.Add("DateDisabled", typeof(DateTime)).AllowDBNull = true;



        }



        internal Task<List<string>> GetDistinctDepartments()

        {

            List<string> departmentList = new List<string>();

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandText = "SomeDataWarehouse.SomeSchema.GetDepartments";

                    cmd.Connection = conn;

                    try

                    {

                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {

                                departmentList.Add(

                                    (string)reader.GetValue(0)

                                    );

                            }

                        }

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error getting distinct departments : {ex.Message}");

                        throw;

                    }

                }

            }



            return Task.FromResult(departmentList);

        }



        internal Task<List<string>> GetDistinctComporLevels()

        {

            List<string> comporLevelList = new List<string>();

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandText = "SomeDataWarehouse.SomeSchema.GetComporLevel";

                    cmd.Connection = conn;

                    try

                    {

                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {

                                comporLevelList.Add(

                                    (string)reader.GetValue(0)

                                    );

                            }

                        }

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error getting distinct Compor levels : {ex.Message}");

                        throw;

                    }

                }

            }



            return Task.FromResult(comporLevelList);

        }



        internal Task<string> AddCompor(NewComporModel newCompor)

        {

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandText = "SomeDataWarehouse.SomeSchema.AddCompor";

                    cmd.Connection = conn;

                    cmd.Parameters.Add("@Testing", SqlDbType.VarChar).Value = Testing;

                    cmd.Parameters.Add("@ComporID", SqlDbType.VarChar).Value = newCompor.ComporID;

                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = newCompor.FirstName;

                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = newCompor.LastName;

                    cmd.Parameters.Add("@ComporLevel", SqlDbType.VarChar).Value = newCompor.ComporLevel;

                    cmd.Parameters.Add("@PercentageOverage", SqlDbType.VarChar).Value = newCompor.PercentageOverage;

                    cmd.Parameters.Add("@DollarOverage", SqlDbType.VarChar).Value = newCompor.DollarOverage;

                    cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = newCompor.Department;

                    cmd.Parameters.Add("@Enabled", SqlDbType.VarChar).Value = newCompor.Enabled;

                    cmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar).Value = newCompor.EmployeeID;

                    try

                    {

                        conn.Open();

                        cmd.ExecuteReader();

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error adding compor {ex.Message}");

                        return Task.FromResult(ex.Message);

                    }

                }

            }

            _logger.LogInformation($"Added user : {newCompor.FirstName} {newCompor.LastName}");

            return Task.FromResult("worked");

        }



        internal Task<NewComporModel> GetACompor(string comporID)

        {

            NewComporModel model = new NewComporModel();

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = $"SELECT TOP(1) ComporID, FirstName, LastName, ComporLevel, PercentOverage, DollarOverage, Department, Enabled, EmployeeID FROM SomeDataWarehouse.SomeSchema.{Table} WHERE ComporID = @ComporID";

                    cmd.Connection = conn;

                    cmd.Parameters.Add("@ComporID", SqlDbType.VarChar).Value = comporID;

                    try

                    {

                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {

                                model.ComporID = (string)reader.GetValue(0);

                                model.FirstName = (string)reader.GetValue(1);

                                model.LastName = (string)reader.GetValue(2);

                                model.ComporLevel = (string)reader.GetValue(3);

                                model.PercentageOverage = (decimal)reader.GetValue(4);

                                model.DollarOverage = (decimal)reader.GetValue(5);

                                model.Department = (string)reader.GetValue(6);

                                model.Enabled = (string)reader.GetValue(7);

                                model.EmployeeID = (string)reader.GetValue(8);

                            }

                        }

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error getting compor with id {comporID} : {ex.Message}");

                        return Task.FromResult(model);

                    }

                }

            }

            return Task.FromResult(model);

        }



        internal Task<string> UpdateCompor(NewComporModel newCompor)

        {

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandText = "SomeDataWarehouse.SomeSchema.UpdateCompor";

                    cmd.Connection = conn;

                    cmd.Parameters.Add("@Testing", SqlDbType.VarChar).Value = Testing;

                    cmd.Parameters.Add("@ComporID", SqlDbType.VarChar).Value = newCompor.ComporID;

                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = newCompor.FirstName;

                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = newCompor.LastName;

                    cmd.Parameters.Add("@ComporLevel", SqlDbType.VarChar).Value = newCompor.ComporLevel;

                    cmd.Parameters.Add("@PercentageOverage", SqlDbType.VarChar).Value = newCompor.PercentageOverage;

                    cmd.Parameters.Add("@DollarOverage", SqlDbType.VarChar).Value = newCompor.DollarOverage;

                    cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = newCompor.Department;

                    cmd.Parameters.Add("@Enabled", SqlDbType.VarChar).Value = newCompor.Enabled;

                    try

                    {

                        conn.Open();

                        cmd.ExecuteReader();

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error updating compor {newCompor.FirstName} {newCompor.LastName} : {ex.Message}");

                        return Task.FromResult(ex.Message);

                    }

                }

            }

            _logger.LogInformation($"Updated user : {newCompor.FirstName} {newCompor.LastName}");

            return Task.FromResult("worked");

        }



        internal Task<string> DeactivateCompor(string comporID)

        {

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = $"UPDATE SomeDataWarehouse.SomeSchema.{Table} SET Enabled = 'N', DateDisabled = @date WHERE ComporID = @comporID";

                    cmd.Connection = conn;

                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = DateTime.Now.Date;

                    cmd.Parameters.Add("@comporID", SqlDbType.NVarChar).Value = comporID;

                    try

                    {

                        conn.Open();

                        cmd.ExecuteReader();

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error deactivating user with comporID: {comporID}");

                        return Task.FromResult(ex.Message);

                    }

                }

            }

            _logger.LogInformation($"Deactivated user with comporID: {comporID}");

            return Task.FromResult("worked");

        }



        internal Task<string> LogComporChange(NewComporModel newCompor, string change, bool deactivated, string user, DateTime timeNow)

        {

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandText = "SomeDataWarehouse.SomeSchema.LogComporChanges";

                    cmd.Connection = conn;

                    cmd.Parameters.Add("@Testing", SqlDbType.VarChar).Value = Testing;

                    cmd.Parameters.Add("@ComporID", SqlDbType.VarChar).Value = newCompor.ComporID;

                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = newCompor.FirstName;

                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = newCompor.LastName;

                    cmd.Parameters.Add("@ComporLevel", SqlDbType.VarChar).Value = newCompor.ComporLevel;

                    cmd.Parameters.Add("@PercentageOverage", SqlDbType.VarChar).Value = newCompor.PercentageOverage;

                    cmd.Parameters.Add("@DollarOverage", SqlDbType.VarChar).Value = newCompor.DollarOverage;

                    cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = newCompor.Department;

                    if (deactivated)

                    {

                        cmd.Parameters.Add("@Enabled", SqlDbType.VarChar).Value = 'N';

                        cmd.Parameters.Add("@DateDisabled", SqlDbType.VarChar).Value = DateTime.Now;

                    }

                    else

                    {

                        cmd.Parameters.Add("@Enabled", SqlDbType.VarChar).Value = newCompor.Enabled;

                        cmd.Parameters.Add("@DateDisabled", SqlDbType.VarChar).Value = DBNull.Value;

                    }

                    cmd.Parameters.Add("@Change", SqlDbType.VarChar).Value = change;

                    cmd.Parameters.Add("@User", SqlDbType.VarChar).Value = StripDomainFromName(user);

                    cmd.Parameters.Add("@TimeOfChange", SqlDbType.VarChar).Value = timeNow;

                    try

                    {

                        conn.Open();

                        cmd.ExecuteReader();

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error adding logs to table ComporDefinitionTableChanges : {ex.Message}");

                        return Task.FromResult(ex.Message);

                    }

                }

            }

            return Task.FromResult("worked");

        }



        internal Task<Tuple<string, string>> GetComporName(string employeeId)

        {

            string AcscID = "";

            string comporName = "";

            using (SqlConnection conn = new SqlConnection(SqlConnectionString))

            {

                using (SqlCommand cmd = new SqlCommand())

                {

                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT TOP(1) employeeID, employeeName FROM SomeDataWarehouse.SomeSchema.EmployeeAccounts WHERE EmployeeId = @employeeId";

                    cmd.Connection = conn;

                    cmd.Parameters.Add("@employeeId", SqlDbType.VarChar).Value = employeeId;

                    try

                    {

                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)

                        {

                            while (reader.Read())

                            {

                                AcscID = (string)reader.GetValue(0);

                                comporName = (string)reader.GetValue(1);

                            }

                        }

                    }

                    catch (Exception ex)

                    {

                        _logger.LogError($"Error getting compor name with compor id {employeeId} : {ex.Message}");

                        return Task.FromResult(new Tuple<string, string>(ex.Message, ""));

                    }

                }

            }



            return Task.FromResult(new Tuple<string, string>(AcscID, comporName));

        }



        public string StripDomainFromName(string name)

        {

            string justUserName = name.Replace("SomeDomain\\", "");

            return justUserName;

        }





    }

}

