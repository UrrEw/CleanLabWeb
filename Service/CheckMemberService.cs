using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWeb.Service
{
    public class CheckMemberService
    {
        private readonly SqlConnection conn;

        public CheckMemberService(SqlConnection connection)
        {
            conn = connection;
        }

        #region 新增
        public void InsertCheckMember(CheckMember newData)
        {
            string sql =$@"INSERT INTO CheckMember(checkmember_id, members_id, homework_id, create_time, create_id, update_time, update_id, is_delete) 
            VALUES(@checkmember_id, @members_id, @homework_id, @create_time, @create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.checkmember_id = Guid.NewGuid();

                cmd.Parameters.AddWithValue("@checkmember_id", newData.checkmember_id);
                cmd.Parameters.AddWithValue("@members_id", newData.members_id);
                cmd.Parameters.AddWithValue("@homework_id", newData.homework_id);
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
        #endregion

        #region 搜尋
         public CheckMember GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,d.*,r.name FROM CheckMember m 
                            INNER JOIN Homework d ON m.homework_id = d.homework_id
                            INNER JOIN Members r ON m.members_id = r.members_id
                            WHERE m.checkmember_id = @Id AND m.is_delete = 0;";
            CheckMember? Data = new CheckMember();

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
                Data.checkmember_id = (Guid)dr["checkmember_id"];
                Data.homework_id = (Guid)dr["homework_id"];
                Data.members_id = (Guid)dr["members_id"];
                Data.name = dr["name"].ToString();
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

        public IEnumerable<CheckMember> GetAllData()
        {
            string sql = $@"SELECT m.*,d.*,r.name FROM CheckMember m 
                            INNER JOIN Homework d ON m.homework_id = d.homework_id
                            INNER JOIN Members r ON m.members_id = r.members_id
                            WHERE m.is_delete = 0;";
                            
            var DataList = new List<CheckMember>();

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
                    CheckMember Data = new CheckMember();
                    Data.checkmember_id = (Guid)dr["checkmember_id"];
                    Data.homework_id = (Guid)dr["homework_id"];
                    Data.members_id = (Guid)dr["members_id"];
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
        #endregion

        #region 修改
        public void UpdateCheckMember(CheckMember updateData)
        {
            string sql = $@"UPDATE CheckMember 
                            SET 
                            homework_id = @homework_id,
                            update_time = @update_time, update_id = @update_id 
                            WHERE 
                            checkmember_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.checkmember_id);
                cmd.Parameters.AddWithValue("@homework_id", updateData.homework_id);
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
        #endregion

        #region 刪除
        public void SoftDeleteCheckMemberById(Guid id)
        {
            string sql = $@"UPDATE CheckMember SET is_delete = 1 WHERE checkmember_id = @Id;";

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
        #endregion
    }
}