using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWeb.Service
{
    public class TesterService
    {
        private readonly SqlConnection conn;

        public TesterService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<Tester> GetAllData()
        {
            string sql = $@"SELECT m.*,d.reservedate,d.reservetime,r.name FROM Tester m
                            INNER JOIN ReserveTime d ON m.reservetime_id = d.reservetime_id
                            INNER JOIN Members r ON m.members_id = r.members_id
                            WHERE m.is_delete = 0;";
            var DataList = new List<Tester>();

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
                    Tester Data = new Tester();
                    Data.tester_id = (Guid)dr["tester_id"];
                    Data.reservetime_id = (Guid)dr["reservetime_id"];
                    Data.members_id = (Guid)dr["members_id"];
                    Data.name = dr["name"].ToString();
                    Data.reservedate = ((DateTime)dr["reservedate"]).Date;
                    DateTime rt = (DateTime)dr["reservetime"];
                    Data.reservetime = rt.TimeOfDay;
                    Data.is_success = Convert.ToBoolean(dr["is_success"]);
                    Data.is_pass = Convert.ToBoolean(dr["is_pass"]);
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

        public void InsertTester(Tester newData)
        {
            string sql = $@"INSERT INTO Tester
                            (tester_id,reservetime_id,members_id,is_success,is_pass,
                            create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@tester_id,@reservetime_id, @members_id, 0,0,
                            @create_time,@create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.tester_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@tester_id", newData.tester_id);
                cmd.Parameters.AddWithValue("@reservetime_id", newData.reservetime_id);
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

        public Tester GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,d.reservedate,d.reservetime,r.name FROM Tester m 
                            INNER JOIN ReserveTime d ON m.reservetime_id = d.reservetime_id
                            INNER JOIN Members r ON m.members_id = r.members_id
                            WHERE m.tester_id = @Id AND m.is_delete = 0;";
            Tester? Data = new Tester();

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
                Data.tester_id = (Guid)dr["tester_id"];
                Data.reservetime_id = (Guid)dr["reservetime_id"];
                Data.members_id = (Guid)dr["members_id"];
                Data.name = dr["name"].ToString();
                Data.reservedate = ((DateTime)dr["reservedate"]).Date;
                DateTime rt = (DateTime)dr["reservetime"];
                Data.reservetime = rt.TimeOfDay;
                Data.is_success = Convert.ToBoolean(dr["is_success"]);
                Data.is_pass = Convert.ToBoolean(dr["is_pass"]);
            }
            catch(Exception e)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
                Console.WriteLine(e.Message);
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }

        public void UpdateTester(Tester updateData)
        {
            string sql = $@"UPDATE Tester 
                            SET 
                            is_success = @is_success,
                            update_time = @update_time,update_id = @update_id 
                            WHERE 
                            tester_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.tester_id);
                cmd.Parameters.AddWithValue("@is_success", updateData.is_success);
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

        public void SoftDeleteTesterById(Guid id)
        {
            string sql = $@"UPDATE Tester SET is_delete = 1 WHERE tester_id = @Id;";

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