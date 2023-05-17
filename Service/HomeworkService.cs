using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace LabWeb.Service
{
    public class HomeworkService
    {
        private readonly SqlConnection conn;

        public HomeworkService(SqlConnection connection)
        {
            conn = connection;
        }
        
        #region 新增
        public void InsertHomework(Homework newData)
        {
            string sql =$@"INSERT INTO Homework(homework_id, check_id, homework_title, homework_content,start_time, 
                end_time, create_time, create_id, update_time, update_id, is_delete) VALUES(@homework_id, @check_id, 
                    @homework_title, @homework_content, @start_time, @end_time, @create_time, @create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.homework_id = Guid.NewGuid();

                cmd.Parameters.AddWithValue("@homework_id", newData.homework_id);
                cmd.Parameters.AddWithValue("@check_id", newData.check_id);
                cmd.Parameters.AddWithValue("@homework_title", newData.homework_title);
                cmd.Parameters.AddWithValue("@homework_content", newData.homework_content);
                cmd.Parameters.AddWithValue("@start_time", newData.start_time);
                cmd.Parameters.AddWithValue("@end_time", newData.end_time);
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
        public Homework GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,r.name FROM Homework m 
                            INNER JOIN Members r ON m.check_id = r.members_id 
                            WHERE m.homework_id = @Id AND m.is_delete = 0;";
            Homework? Data = new Homework();

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
                Data.homework_id = (Guid)dr["homework_id"];
                Data.check_id = (Guid)dr["check_id"];
                Data.homework_title = dr["homework_title"].ToString();
                Data.homework_content = dr["homework_content"].ToString();
                Data.start_time = ((DateTime)dr["start_time"]).Date;
                Data.end_time = ((DateTime)dr["end_time"]).Date;
                Data.name = dr["name"].ToString();
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

        public List<Homework> GetDataByName(Guid Id)
        {
            string sql = $@"SELECT m.*,r.name FROM Homework m 
                            INNER JOIN Members r ON m.check_id = r.members_id 
                            WHERE m.check_id = @Id AND m.is_delete = 0;";
             var DataList = new List<Homework>();

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
                 while (dr.Read())
                {
                    Homework Data = new Homework();
                    Data.homework_id = (Guid)dr["homework_id"];
                    Data.check_id = (Guid)dr["check_id"];
                    Data.homework_title = dr["homework_title"].ToString();
                    Data.homework_content = dr["homework_content"].ToString();
                    Data.start_time = ((DateTime)dr["start_time"]).Date;
                    Data.end_time = ((DateTime)dr["end_time"]).Date;
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

        public IEnumerable<Homework> GetAllData()
        {
            string sql = $@"SELECT m.*,r.name FROM Homework m
                            INNER JOIN Members r ON m.check_id = r.members_id
                            WHERE m.is_delete = 0;";
             var DataList = new List<Homework>();

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                 while (dr.Read())
                {
                        Homework Data = new Homework();
                        Data.homework_id = (Guid)dr["homework_id"];
                        Data.check_id = (Guid)dr["check_id"];
                        Data.homework_title = dr["homework_title"].ToString();
                        Data.homework_content = dr["homework_content"].ToString();
                        Data.start_time = ((DateTime)dr["start_time"]).Date;
                        Data.end_time = ((DateTime)dr["end_time"]).Date;
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
         public void UpdateHomework(Homework updateData)
        {
            string sql = $@"UPDATE Homework 
                            SET 
                            homework_title = @homework_title,homework_content = @homework_content,
                            start_time = @start_time, end_time = @end_time,
                            update_time = @update_time, update_id = @update_id
                            WHERE 
                            homework_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.homework_id);
                cmd.Parameters.AddWithValue("@homework_title", updateData.homework_title);
                cmd.Parameters.AddWithValue("@homework_content", updateData.homework_content);
                cmd.Parameters.AddWithValue("@start_time", updateData.start_time);
                cmd.Parameters.AddWithValue("@end_time",  updateData.end_time);
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
        public void SoftDeleteHomeworkById(Guid id)
        {
            string sql = $@"UPDATE Homework SET is_delete = 1 WHERE homework_id = @Id;";

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