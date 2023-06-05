using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;
using LabWeb.ViewModel;

namespace LabWeb.Service
{
    public class TestReserveService
    {
        private readonly SqlConnection conn;

        public TestReserveService(SqlConnection connection)
        {
            conn = connection;
        }

        public void InsertTestReserve(TestReserve newData)
        {
            string sql = $@"INSERT INTO TestReserve
                            (testreserve_id,test_id,
                            create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@testreserve_id, @test_id,
                            @create_time,@create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.testreserve_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@testreserve_id", newData.testreserve_id);
                cmd.Parameters.AddWithValue("@test_id", newData.test_id);
                cmd.Parameters.AddWithValue("@create_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@create_id", newData.create_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newData.update_id);

                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public TestReserve GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,d.test_id FROM TestReserve m 
                            INNER JOIN Test d ON m.test_id = d.test_id
                            WHERE m.testreserve_id = @Id AND m.is_delete=0;";
            TestReserve? Data = new TestReserve();

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.test_id = (Guid)dr["test_id"];
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }

        public List<TestReserveViewModel> GetAllTestReserveData()
        {
            string sql = $@"SELECT DISTINCT p.*,r.*,t.*,d.test_title,mp.name AS proctor_name, mt.name AS tester_name FROM Proctor p
                            INNER JOIN Test d ON p.test_id = d.test_id
                            INNER JOIN ReserveTime r ON p.proctor_id = r.proctor_id
                            INNER JOIN Tester t ON t.reservetime_id = r.reservetime_id
                            INNER JOIN Members mp ON mp.members_id = p.members_id
                            INNER JOIN Members mt ON mt.members_id = t.members_id
                            WHERE p.is_delete=0 AND t.is_delete=0 AND r.is_delete=0
                            ORDER BY r.reservedate,r.reservetime ;";
            List<TestReserveViewModel> DataList = new List<TestReserveViewModel>();

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    TestReserveViewModel Data = new TestReserveViewModel();
                    Data.proctor_id = (Guid)dr["proctor_id"];
                    Data.test_id = (Guid)dr["test_id"];
                    Data.members_id = (Guid)dr["members_id"];
                    Data.proctor_name = dr["proctor_name"].ToString();
                    Data.tester_name = dr["tester_name"].ToString();
                    Data.test_title = dr["test_title"].ToString();
                    Data.create_id = (Guid)dr["create_id"];
                    Data.update_id = (Guid)dr["update_id"];
                    Data.reservetime_id = (Guid)dr["reservetime_id"];
                    Data.reservedate = ((DateTime)dr["reservedate"]).Date;
                    DateTime rt = (DateTime)dr["reservetime"];
                    Data.reservetime = rt.TimeOfDay;
                    Data.tester_id = (Guid)dr["tester_id"];
                    Data.is_success = Convert.ToBoolean(dr["is_success"]);
                    Data.is_fail = Convert.ToBoolean(dr["is_pass"]);
                    Data.is_delete = Convert.ToBoolean(dr["is_delete"]);
                    DataList.Add(Data);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
    }
}