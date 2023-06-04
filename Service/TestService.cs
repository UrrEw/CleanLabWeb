using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWeb.Service
{
    public class TestService
    {
        private readonly SqlConnection conn;
        private readonly GetLoginClaimService _getLoginClaimService;

        public TestService(SqlConnection connection,GetLoginClaimService getLoginClaimService)
        {
            conn = connection;
            _getLoginClaimService = getLoginClaimService;
        }

        public IEnumerable<Test> GetAllData(Guid Id)
        {
            string sql = $@"SELECT m.*,t.is_success,d.name FROM Test m 
                            LEFT JOIN Tester t ON m.test_id = t.test_id AND t.members_id = @Id
                            INNER JOIN Members d ON d.members_id = m.create_id
                            WHERE m.is_delete = 0 ORDER BY end_date;";
            var DataList = new List<Test>();

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
                while(dr.Read())
                {
                    Test Data = new Test();
                    Data.test_id = (Guid)dr["test_id"];
                    Data.test_title = dr["test_title"].ToString();
                    Data.test_content = dr["test_content"].ToString();
                    Data.start_date = ((DateTime)dr["start_date"]).Date;
                    Data.end_date = ((DateTime)dr["end_date"]).Date;
                    Data.create_id = (Guid)dr["create_id"];
                    if (dr["is_success"] != DBNull.Value)
                    {
                        Data.is_success = Convert.ToBoolean(dr["is_success"]);
                        if (Data.is_success)
                        {
                            Data.Status = "預約成功";
                        }
                        else
                        {
                            Data.Status = "尚未預約";
                        }
                    }
                    else
                    {
                        Data.is_success = false;
                        Data.Status = "尚未預約";
                    }
                    Data.name = dr["name"].ToString();
                    
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return DataList;
        }

        public void InsertTest(Test newData)
        {
            string sql = $@"INSERT INTO Test
                            (test_id,test_title,test_content,start_date,end_date,
                            create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@test_id, @test_title, @test_content,@start_date,@end_date,
                            @create_time,@create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.test_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@test_id", newData.test_id);
                cmd.Parameters.AddWithValue("@test_title", newData.test_title);
                cmd.Parameters.AddWithValue("@test_content", newData.test_content);
                cmd.Parameters.AddWithValue("@start_date", newData.start_date);
                cmd.Parameters.AddWithValue("@end_date", newData.end_date);
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

        public Test GetDataById(Guid Id)
        {
            string sql = $@"SELECT * FROM Test WHERE test_id = @Id AND is_delete=0;";
            Test? Data = new Test();

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
                Data.test_title = dr["test_title"].ToString();
                Data.test_content = dr["test_content"].ToString();
                Data.start_date = ((DateTime)dr["start_date"]).Date;
                Data.end_date = ((DateTime)dr["end_date"]).Date;
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

        public void UpdateTest(Test updateData)
        {
            string sql = $@"UPDATE Test 
                            SET 
                            test_title = @test_title,test_content = @test_content, end_date = @end_date,
                            start_date = @start_date,update_time = @update_time,update_id = @update_id 
                            WHERE 
                            test_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.test_id);
                cmd.Parameters.AddWithValue("@test_title", updateData.test_title);
                cmd.Parameters.AddWithValue("@test_content", updateData.test_content);
                cmd.Parameters.AddWithValue("@end_date", updateData.end_date);
                cmd.Parameters.AddWithValue("@start_date", updateData.start_date);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", updateData.update_id);
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

        public void SoftDeleteTestById(Guid id)
        {
            string sql = $@"UPDATE Test SET is_delete = 1 WHERE test_id = @Id;";

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public string GetTesterStatus(Guid Id)
        {
            var sql = @$"SELECT is_success FROM Tester WHERE members_id = @Id ;";
            var result = string.Empty;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                var is_success = Convert.ToBoolean(dr["is_success"]);
                if(is_success)
                {
                    result = "預約成功";
                }
                else
                {
                    result = "尚未預約";
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return result;
        }
    }
}