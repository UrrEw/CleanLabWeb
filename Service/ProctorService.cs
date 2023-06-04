using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWeb.Service
{
    public class ProctorService
    {
        private readonly SqlConnection conn;

        public ProctorService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<Proctor> GetAllData()
        {
            string sql = $@"SELECT m.*,d.test_title,r.name FROM Proctor m 
                            INNER JOIN Test d ON m.test_id = d.test_id
                            INNER JOIN Members r ON m.members_id = r.members_id
                            WHERE m.is_delete = 0;";
            var DataList = new List<Proctor>();

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
                    Proctor Data = new Proctor();
                    Data.proctor_id = (Guid)dr["proctor_id"];
                    Data.test_id = (Guid)dr["test_id"];
                    Data.members_id = (Guid)dr["members_id"];
                    Data.name = dr["name"].ToString();
                    Data.test_title = dr["test_title"].ToString();
                    Data.create_id = (Guid)dr["create_id"];
                    Data.update_id = (Guid)dr["update_id"];
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

        public void InsertProctor(Proctor newData)
        {
            string sql = $@"INSERT INTO Proctor
                            (proctor_id,test_id,members_id,
                            create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@proctor_id, @test_id, @members_id,
                            @create_time,@create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.proctor_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@proctor_id", newData.proctor_id);
                cmd.Parameters.AddWithValue("@test_id", newData.test_id);
                cmd.Parameters.AddWithValue("@members_id", newData.members_id);
                cmd.Parameters.AddWithValue("@create_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@create_id", newData.create_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newData.update_id);

                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public Proctor GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,d.test_title,r.name FROM Proctor m 
                            INNER JOIN Test d ON m.test_id = d.test_id
                            INNER JOIN Members r ON m.members_id = r.members_id
                            WHERE m.proctor_id = @Id AND m.is_delete=0;";
            Proctor? Data = new Proctor();

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
                Data.proctor_id = (Guid)dr["proctor_id"];
                Data.test_id = (Guid)dr["test_id"];
                Data.members_id = (Guid)dr["members_id"];
                Data.test_title = dr["test_title"].ToString();
                Data.name = dr["name"].ToString();
                Data.create_id = (Guid)dr["create_id"];
                Data.update_id = (Guid)dr["update_id"];
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

        public void UpdateProctor(Proctor updateData)
        {
            string sql = $@"UPDATE Proctor 
                            SET 
                            test_id = @test_id,members_id = @members_id,
                            update_time = @update_time,update_id = @update_id 
                            WHERE 
                            proctor_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.proctor_id);
                cmd.Parameters.AddWithValue("@test_id", updateData.test_id);
                cmd.Parameters.AddWithValue("@members_id", updateData.members_id);
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

        public void SoftDeleteProctorById(Guid id)
        {
            string sql = $@"UPDATE Proctor SET is_delete = 1 WHERE proctor_id = @Id;";

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
    }
}