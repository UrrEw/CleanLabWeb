using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;

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
    }
}