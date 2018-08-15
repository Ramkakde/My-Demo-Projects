using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace SignUpService.DataAccess
{
    public class BusinessCommonFunctions
    {
        public static string GetAppSettings(string Key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[Key];
        }

        public static string GetCDRConnections(string p_strPracticeUid)
        {
            string _strSqlCon = "";
            StringBuilder _sbQuery = new StringBuilder();
            _sbQuery.Append(" SELECT ServerName,LoginName,Convert(varchar,Password) as Password ,DatabaseName FROM ");
            _sbQuery.Append(" Practice P INNER JOIN ClinicalDataRepository C ");
            _sbQuery.Append(" ON P.ClinicalDataRepositoryUid = C.ClinicalDataRepositoryUid ");
            _sbQuery.AppendFormat(" WHERE P.PracticeUid = '{0}'", p_strPracticeUid);

            DataTable _dt = null;

            using (DBHelper DBConn = new DBHelper("FIGMDHQIManagement"))
            {
                _dt = DBConn.GetDatatable(_sbQuery.ToString());
            }
            if (_dt != null && _dt.Rows.Count > 0)
            {
                string _strServerName = ""; string _strLoginName = ""; string _strPassword = ""; string _strDbName = "";
                if (_dt.Rows[0]["ServerName"] != null)
                    _strServerName = _dt.Rows[0]["ServerName"].ToString();
                if (_dt.Rows[0]["LoginName"] != null)
                    _strLoginName = _dt.Rows[0]["LoginName"].ToString();
                if (_dt.Rows[0]["Password"] != null)
                    _strPassword = _dt.Rows[0]["Password"].ToString();
                if (_dt.Rows[0]["DatabaseName"] != null)
                    _strDbName = _dt.Rows[0]["DatabaseName"].ToString();


                _strSqlCon = GetSqlServerConnectionString(_strServerName, _strDbName, _strLoginName, _strPassword);
            }
            return _strSqlCon;

        }

        public static string GetSqlServerConnectionString(string DataSource, string InitialCatlog, string UserId, string Password)
        {
            if (!string.IsNullOrEmpty(InitialCatlog))
                return string.Format(@"data source = {0}; initial catalog = {1};user id = {2}; password = {3};", DataSource, InitialCatlog, UserId, Password);
            else
                return string.Format(@"data source = {0}; user id = {1}; password = {2};", DataSource, UserId, Password);
        }
    }

    public class DBHelper : IDisposable
    {
        SqlConnection Conn;
        MySqlConnection _dbCon;

        public enum DatabaseType
        {
            SQLServer,
            Oracle,
            PostgreSQL,
            MySQL,
            UnKnown
        }

        public DBHelper(string ConnectTo)
        {
            Conn = new SqlConnection(BusinessCommonFunctions.GetAppSettings(ConnectTo));
        }

        public DBHelper(Guid PracticeUid)
        {
            string _strPracticeUid = PracticeUid.ToString();
            if(!string.IsNullOrEmpty(_strPracticeUid ))
                Conn = new SqlConnection(BusinessCommonFunctions.GetCDRConnections(_strPracticeUid));
        }

        public DBHelper(string strCon, DatabaseType _dbType)
        {
            if(_dbType == DatabaseType.MySQL)
                _dbCon = new MySqlConnection(strCon);
        }

        private IDbDataParameter[] GetBaseTypeParameter(FIGDbParameter[] Parameters)
        {
            IDbDataParameter[] P = new IDbDataParameter[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
            {
                P[i] = GetBaseTypeParameter(Parameters[i]);
            }
            return P;
        }

        private IDbDataParameter GetBaseTypeParameter(FIGDbParameter Parameter)
        {
            IDbDataParameter P = null;
            P = new OleDbParameter();
            P.ParameterName = Parameter.Name;
            P.Value = Parameter.Value;
            P.DbType = Parameter.PType;
            //P.IsNullable = Parameter.IsNullable;
            P.Direction = Parameter.Direction;
            if (Parameter.Size > 0)
                P.Size = Parameter.Size;

            return P;
        }

        public DataTable GetDatatable(string strQuery)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandText = strQuery;
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;
            DataTable ds = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(ds);
            }

            return ds;
        }

        public DataSet GetDataset(string Query)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandText = Query;
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;

            DataSet _dataset = new DataSet();
            _dataset.EnforceConstraints = false;

            SqlDataAdapter _dataadapter = new SqlDataAdapter(cmd);
            _dataadapter.Fill(_dataset);
            _dataadapter = null;
            return _dataset;
        }

        public object GetScalar(string strQuery)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandText = strQuery;
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;

            return cmd.ExecuteScalar();
        }

        public int ExecuteNonQuery(SqlCommand cmd)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            cmd.Connection = Conn;
            int _return = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return _return;
        }



        public int ExecuteNonQuery(string strQuery)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;

            cmd.CommandText = strQuery;
          int _return =  cmd.ExecuteNonQuery();
          return _return;
        }

        public void ExecuteNonQuery(string strQuery, FIGDbParameter[] ParamColl)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;
            IDbDataParameter[] Params = GetBaseTypeParameter(ParamColl);
            for (int K = 0; K < Params.Length; K++)
                cmd.Parameters.Add(Params[K]);

            cmd.CommandText = strQuery;
            cmd.ExecuteNonQuery();
        }

        public void ExecuteNonQuery(string strQuery, SqlParameter[] ParamColl)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;
            cmd.Parameters.AddRange(ParamColl);

            cmd.CommandText = strQuery;
            cmd.ExecuteNonQuery();

        }

        public void Dispose()
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
                Conn.Dispose();
            }
        }

        public DataTable GetDatatableMySql(string strQuery)
        {
            if (_dbCon.State == ConnectionState.Closed)
            {
                _dbCon.Open();
            }
            MySqlCommand _command = new MySqlCommand();
            _command.CommandType = CommandType.Text;
            _command.CommandText = strQuery;
            _command.Parameters.Clear();
            _command.Connection = _dbCon;
            _command.CommandTimeout = 0;

            DataSet _dataset = new DataSet();
            _dataset.EnforceConstraints = false;

            using (DbDataAdapter _dataadapter = GetAdapter(DatabaseType.MySQL,_command))
            {
                _dataset.Clear();
                _dataadapter.Fill(_dataset);
            }
            return _dataset.Tables[0];
        }

        public int ExecuteNonQueryMysql(string strQuery)
        {
            if (_dbCon.State == ConnectionState.Closed)
            {
                _dbCon.Open();
            }
            MySqlCommand _command = new MySqlCommand();
            _command.CommandType = CommandType.Text;
            _command.Parameters.Clear();
            _command.Connection = _dbCon;
            _command.CommandTimeout = 0;

            _command.CommandText = strQuery;
          int _return =  _command.ExecuteNonQuery();

          return _return;
        }

        public object GetScalarMySql(string strQuery)
        {
            if (_dbCon.State == ConnectionState.Closed)
            {
                _dbCon.Open();
            }

            MySqlCommand _command = new MySqlCommand();
            _command.CommandType = CommandType.Text;
            _command.Parameters.Clear();
            _command.CommandText = strQuery;
            _command.Connection = _dbCon;
            _command.CommandTimeout = 0;

            return _command.ExecuteScalar();
        }

        private DbDataAdapter GetAdapter(DatabaseType _databasetype, DbCommand _command)
        {
            if (_databasetype == DatabaseType.SQLServer)
                return new OleDbDataAdapter((OleDbCommand)_command);
            else if (_databasetype == DatabaseType.MySQL)
                return new MySqlDataAdapter((MySqlCommand)_command);
            return null;
        }

        /// <summary>
        /// Return SQLDataReader Object. This method is only for without parameter.
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string strQuery)
        {
            SqlDataReader _return;

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;

            cmd.CommandText = strQuery;
            _return = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return _return;
        }

        /// <summary>
        /// Return SQLDataReader Object.
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="ParamColl"></param>
        /// <returns></returns>
        /// 

        public DataTable GetDataTable(SqlCommand sqlCommand)
        {
            DataTable _return = new DataTable();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            using (sqlCommand)
            {
                sqlCommand.CommandTimeout = 0;
                sqlCommand.Connection = Conn;
                sqlCommand.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                {
                    adapter.Fill(_return);
                    sqlCommand.Connection.Close();
                }
            }

            return _return;
        }


        public SqlDataReader ExecuteReader(string strQuery, FIGDbParameter[] ParamColl)
        {
            SqlDataReader _return;

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandTimeout = 0;
            cmd.Connection = Conn;
            IDbDataParameter[] Params = GetBaseTypeParameter(ParamColl);

            for (int K = 0; K < Params.Length; K++)
                cmd.Parameters.Add(Params[K]);

            cmd.CommandText = strQuery;
            _return = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return _return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(SqlCommand cmmand)
        {
            SqlDataReader _return;

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }

            cmmand.CommandTimeout = 0;
            cmmand.Connection = Conn;

            _return = cmmand.ExecuteReader(CommandBehavior.CloseConnection);

            return _return;
        }
    }

    public class FIGDbParameter
    {
        #region Private Members
        private string _parametername = string.Empty;
        private object _parametervalue = null;
        private DbType _parametertype = DbType.String;
        private int _parametersize = 0;
        private bool _isnullable = false;
        private ParameterDirection _direction = ParameterDirection.Input;
        #endregion

        #region Constructors
        public FIGDbParameter(string Name, object Value)
        {
            this._parametername = Name;
            this._parametervalue = Value;
        }

        public FIGDbParameter(string Name, object Value, DbType PType)
            : this(Name, Value)
        {
            this._parametertype = PType;
        }

        public FIGDbParameter(string Name, object Value, DbType PType, int Size)
            : this(Name, Value, PType)
        {
            this._parametersize = Size;
        }

        public FIGDbParameter(string Name, DbType PType, int Size)
            : this(Name, null, PType, Size)
        {
        }

        public FIGDbParameter(string Name, DbType PType)
            : this(Name, null, PType)
        {
        }

        public FIGDbParameter(string Name, object Value, DbType PType, int Size, bool IsNullable)
            : this(Name, Value, PType, Size)
        {
            this._isnullable = IsNullable;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Name of the parameter
        /// </summary>
        /// <author>Abhijit Mane</author>
        public string Name
        {
            get { return _parametername; }
            set { _parametername = value; }
        }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        /// <author>Abhijit Mane</author>
        public object Value
        {
            get { return _parametervalue; }
            set { _parametervalue = value; }
        }

        /// <summary>
        /// Type of parameter
        /// </summary>
        /// <author>Abhijit Mane</author>
        public DbType PType
        {
            get { return _parametertype; }
            set { _parametertype = value; }
        }

        /// <summary>
        /// Size of parameter
        /// </summary>
        /// <author>Abhijit Mane</author>
        public int Size
        {
            get { return _parametersize; }
            set { _parametersize = value; }
        }

        /// <summary>
        /// Gets and sets whether the parameter is nullable
        /// </summary>
        /// <author>Abhijit Mane</author>
        public bool IsNullable
        {
            get { return _isnullable; }
            set { _isnullable = value; }
        }

        /// <summary>
        /// Gets and Sets the parameter's direction
        /// </summary>
        /// <author>Abhijit Mane</author>
        public ParameterDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        #endregion
    }
}