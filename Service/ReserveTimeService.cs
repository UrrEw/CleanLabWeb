using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace LabWeb.Service
{
    public class ReserveTimeService
    {
        private readonly SqlConnection conn;

        public ReserveTimeService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<ReserveTime> GetAllData()
        {
            string sql = $@"SELECT m.* FROM ReserveTime m
                            INNER JOIN Proctor d ON m.proctor_id = d.proctor_id
                            WHERE m.is_delete = 0;";
            var DataList = new List<ReserveTime>();

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
                    ReserveTime Data = new ReserveTime();
                    Data.reservetime_id = (Guid)dr["reservetime_id"];
                    Data.proctor_id = (Guid)dr["proctor_id"];
                    Data.reservedate = ((DateTime)dr["reservedate"]).Date;
                    DateTime rt = (DateTime)dr["reservetime"];
                    Data.reservetime = rt.TimeOfDay;
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

        public void InsertReserveTime(ReserveTime newData)
        {
            string sql = $@"INSERT INTO ReserveTime
                            (reservetime_id,proctor_id,reservedate,reservetime,
                            create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@reservetime_id, @proctor_id,@reservedate, @reservetime,
                            @create_time,@create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.reservetime_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@reservetime_id", newData.reservetime_id);
                cmd.Parameters.AddWithValue("@proctor_id", newData.proctor_id);
                cmd.Parameters.AddWithValue("@reservedate", newData.reservedate);
                cmd.Parameters.AddWithValue("@reservetime", newData.reservetime);
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

        public ReserveTime GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.* FROM ReserveTime m 
                            INNER JOIN Proctor d ON m.proctor_id = d.proctor_id
                            WHERE m.reservetime_id = @Id AND m.is_delete=0;";
            ReserveTime? Data = new ReserveTime();

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
                Data.reservetime_id = (Guid)dr["reservetime_id"];
                Data.proctor_id = (Guid)dr["proctor_id"];
                Data.reservedate = ((DateTime)dr["reservedate"]).Date;
                DateTime rt = (DateTime)dr["reservetime"];
                Data.reservetime = rt.TimeOfDay;
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

        public void UpdateReserveTime(ReserveTime updateData)
        {
            string sql = $@"UPDATE ReserveTime 
                            SET 
                            reservedate = @reservedate,reservetime = @reservetime,proctor_id = @proctor_id,
                            update_time = @update_time,update_id = @update_id 
                            WHERE 
                            reservetime_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.reservetime_id);
                cmd.Parameters.AddWithValue("@proctor_id", updateData.proctor_id);
                cmd.Parameters.AddWithValue("@reservedate", updateData.reservedate);
                cmd.Parameters.AddWithValue("@reservetime", updateData.reservetime);
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

        public void SoftDeleteReserveTimeById(Guid id)
        {
            string sql = $@"UPDATE ReserveTime SET is_delete = 1 WHERE reservetime_id = @Id;";

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