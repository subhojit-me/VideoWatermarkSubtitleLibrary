using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideowatermarkLibrary.Model;
using VideoWatermarkService.Model;

namespace VideowatermarkLibrary.Helper
{
    public class WaterMarkGeneratorDbHelper
    {
        public OperationStatus GetNonConvertedDatas()
        {
            System.Data.DataTable datas = new System.Data.DataTable();
            try
            {
                var query = "SELECT * FROM VIDEO_WATERMARK WHERE IS_PROCESSED = 0 AND IS_PROCESSING = 0";
                using (var adapter = new SqlDataAdapter(query,ConnectionStringHelper.getSqlConnection()))
                {
                    adapter.Fill(datas);
                }
                if (datas == null && datas.Rows.Count == 0)
                    return new OperationStatus(true, null);
                else
                    return new OperationStatus(true, VIDEO_WATERMARK_DTO.ToList(datas));
            }
            catch (Exception ex)
            {
                return new OperationStatus(false, error: ex.Message);
            }
        }
        
        public OperationStatus GetNonConvertedDatas(bool updateWorkingStatus = false)
        {
            try
            {
                if (!updateWorkingStatus)
                    return GetNonConvertedDatas();

                var status = GetNonConvertedDatas();
                if (!status.IsSuccess || status.Data == null || (status.Data as List<VIDEO_WATERMARK_DTO>).Count == 0)
                    return status;
                int rowsEffected;
                SqlTransaction transaction = null;
                if (status.IsSuccess)
                {
                    var keys = (status.Data as List<VIDEO_WATERMARK_DTO>).Select(dto => $"'{dto.UDTUNIQUEROW}'").ToArray();
                    var uniquerows = string.Join(",", keys);
                    var query = @"
UPDATE VIDEO_WATERMARK 
SET IS_PROCESSED = 0, 
	IS_PROCESSING = 1
WHERE UDTUNIQUEROW in (@UDTUNIQUEROWS)
";
                    var connection = ConnectionStringHelper.getSqlConnection();
                    try
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();
                        using (var command = new SqlCommand(query, connection, transaction))
                        {
                            //command.Parameters.Add(new SqlParameter("@UDTUNIQUEROWS", keys));
                            command.CommandText = command.CommandText.Replace("@UDTUNIQUEROWS", uniquerows);
                            rowsEffected = command.ExecuteNonQuery();
                        }
                        if (rowsEffected == (status.Data as List<VIDEO_WATERMARK_DTO>).Count)
                        {
                            transaction.Commit();
                            return status;
                        }
                        else
                        {
                            transaction.Rollback();
                            return new OperationStatus(false, status.Data);
                        }
                    }
                    catch (Exception ex)
                    {
                        if(transaction != null)
                            transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                {
                    return status;
                }
            }
            catch (Exception ex)
            {
                return new OperationStatus(false, "Unhandled Exception", ex.Message);
            }
        }

        public OperationStatus UpdateWatermarkProcessingDetails(VIDEO_WATERMARK_DTO obj)
        {
            int effectedRows;
            SqlTransaction transaction = null;
            var connection = ConnectionStringHelper.getSqlConnection();
            try
            {
                var query = @"
UPDATE VIDEO_WATERMARK 
SET IS_PROCESSED = @IS_PROCESSED,
    IS_PROCESSING = @IS_PROCESSING,
	CONVERTED_DT_STAMP = @CONVERTED_DT_STAMP,
    ERROR_MESSAGE = @ERROR_MESSAGE
WHERE UDTUNIQUEROW = @UDTUNIQUEROW
";
                connection.Open();
                transaction = connection.BeginTransaction();
                using (var command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddRange(new SqlParameter[]{
                        new SqlParameter("@UDTUNIQUEROW", obj.UDTUNIQUEROW),
                        new SqlParameter("@IS_PROCESSED", Convert.ToInt32(obj.IS_PROCESSED)),
                        new SqlParameter("@IS_PROCESSING", Convert.ToInt32(obj.IS_PROCESSING)),
                        new SqlParameter("@CONVERTED_DT_STAMP", (object)obj.CONVERTED_DT_STAMP ?? DBNull.Value),
                        new SqlParameter("@ERROR_MESSAGE", (object)obj.ERROR_MESSAGE ?? DBNull.Value)
                    });
                    effectedRows = command.ExecuteNonQuery();
                }
                if(effectedRows == 1)
                {
                    transaction.Commit();
                    return new OperationStatus(true, $"Successfully updated {effectedRows} Rows");
                }
                else
                {
                    transaction.Rollback();
                    return new OperationStatus(false, $"Executed {effectedRows} Records");
                }
            }
            catch (Exception ex)
            {
                if(transaction != null)
                    transaction.Rollback();
                return new OperationStatus(false, "Exception Occoured", ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        
        
    }
}
