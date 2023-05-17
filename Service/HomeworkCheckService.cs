using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWeb.Service
{
    public class HomeworkCheckService
    {
        private readonly SqlConnection conn;

        public HomeworkCheckService(SqlConnection connection)
        {
            conn = connection;
        }

        #region 新增
        public void InsertHomeworkCheck(HomeworkCheck newData)
        {
            string sql =$@"INSERT INTO HomeworkCheck
                            (homeworkcheck_id, homework_id, student_name, check_member, check_result, check_note,
                            finishtime, check_file, create_time, create_id, update_time, update_id, is_delete) 
                            VALUES
                            (@homeworkcheck_id, @homework_id, @student_name, @check_member,@check_result, @check_note, 
                            @finishtime, @check_file, @create_time, @create_id, @update_time, @update_id, 0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.homeworkcheck_id = Guid.NewGuid();

                cmd.Parameters.AddWithValue("@homeworkcheck_id", newData.homeworkcheck_id);
                cmd.Parameters.AddWithValue("@homework_id", newData.homework_id);
                cmd.Parameters.AddWithValue("@student_name", newData.student_name);
                cmd.Parameters.AddWithValue("@check_member", newData.check_member);
                cmd.Parameters.AddWithValue("@check_result", newData.check_result);
                cmd.Parameters.AddWithValue("@check_note", newData.check_note);
                cmd.Parameters.AddWithValue("@finishtime", DateTime.Now);
                cmd.Parameters.AddWithValue("@check_file", newData.check_file);
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
        public HomeworkCheck GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,d.*,r.* FROM HomeworkCheck m 
                            INNER JOIN Homework d ON m.homework_id = d.homework_id
                            INNER JOIN Members r ON m.student_name = r.members_id
                            WHERE m.homeworkcheck_id = @Id AND m.is_delete = 0;";
            HomeworkCheck? Data = new HomeworkCheck();

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
                Data.homeworkcheck_id = (Guid)dr["homeworkcheck_id"];
                Data.homework_id = (Guid)dr["homework_id"];
                Data.student_name = (Guid)dr["student_name"];
                Data.check_member = (Guid)dr["check_member"];
                Data.check_member = (Guid)dr["check_member"];
                Data.check_result = (bool)dr["check_result"];
                Data.name = dr["name"].ToString();
                Data.finishtime = ((DateTime)dr["finishtime"]).Date;
                Data.check_file = dr["check_file"].ToString();
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

         public List<HomeworkCheck> GetDataByHomework(Guid Id)
        {
            string sql = $@"SELECT m.*,d.*,r.name FROM HomeworkCheck m 
                            INNER JOIN Homework d ON m.homework_id = d.homework_id
                            INNER JOIN Members r ON m.student_name = r.members_id 
                            WHERE m.homework_id = @Id AND m.is_delete = 0;";

             var DataList = new List<HomeworkCheck>();

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
                    HomeworkCheck Data = new HomeworkCheck();
                    Data.homeworkcheck_id = (Guid)dr["homeworkcheck_id"];
                    Data.homework_id = (Guid)dr["homework_id"];
                    Data.student_name = (Guid)dr["student_name"];
                    Data.check_member = (Guid)dr["check_member"];
                    Data.check_result = (bool)dr["check_result"];
                    Data.name = dr["name"].ToString();
                    Data.check_note = dr["check_note"].ToString();
                    Data.finishtime = ((DateTime)dr["finishtime"]).Date;
                    Data.check_file = dr["check_file"].ToString();
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

        public List<HomeworkCheck> GetDataByName(Guid Id)
        {
            string sql = $@"SELECT m.*,d.*,r.name FROM HomeworkCheck m 
                            INNER JOIN Homework d ON m.homework_id = d.homework_id
                            INNER JOIN Members r ON m.student_name = r.members_id 
                            WHERE m.student_name = @Id AND m.is_delete = 0;";

             var DataList = new List<HomeworkCheck>();

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
                    HomeworkCheck Data = new HomeworkCheck();
                    Data.homeworkcheck_id = (Guid)dr["homeworkcheck_id"];
                    Data.homework_id = (Guid)dr["homework_id"];
                    Data.student_name = (Guid)dr["student_name"];
                    Data.check_member = (Guid)dr["check_member"];
                    Data.check_result = (bool)dr["check_result"];
                    Data.name = dr["name"].ToString();
                    Data.check_note = dr["check_note"].ToString();
                    Data.finishtime = ((DateTime)dr["finishtime"]).Date;
                    Data.check_file = dr["check_file"].ToString();
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
        
        public IEnumerable<HomeworkCheck> GetAllData()
        {
            string sql = $@"SELECT m.*,d.*,r.* FROM HomeworkCheck m
                            INNER JOIN Homework d ON m.homework_id = d.homework_id
                            INNER JOIN Members r ON m.student_name = r.members_id
                            WHERE m.is_delete=0;";

            var DataList = new List<HomeworkCheck>();

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
                    HomeworkCheck Data = new HomeworkCheck();
                    Data.homeworkcheck_id = (Guid)dr["homeworkcheck_id"];
                    Data.homework_id = (Guid)dr["homework_id"];
                    Data.student_name = (Guid)dr["student_name"];
                    Data.check_member = (Guid)dr["check_member"];
                    Data.check_result = (bool)dr["check_result"];
                    Data.name = dr["name"].ToString();
                    Data.check_note = dr["check_note"].ToString();
                    Data.finishtime = ((DateTime)dr["finishtime"]).Date;
                    Data.check_file = dr["check_file"].ToString();
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
        public void UpdateHomeworkCheck(HomeworkCheck updateData)
        {
            string sql = $@"UPDATE HomeworkCheck 
                            SET 
                            check_file = @check_file,
                            check_note = @check_note, finishtime = @finishtime,
                            update_time = @update_time, update_id = @update_id
                            WHERE 
                            homeworkcheck_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.homeworkcheck_id);
                cmd.Parameters.AddWithValue("@check_file", updateData.check_file);
                cmd.Parameters.AddWithValue("@check_note", updateData.check_note);
                cmd.Parameters.AddWithValue("@finishtime",  DateTime.Now);
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
        public void SoftDeleteHomeworkCheckById(Guid id)
        {
            string sql = $@"UPDATE HomeworkCheck SET is_delete = 1 WHERE homeworkcheck_id = @Id;";

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