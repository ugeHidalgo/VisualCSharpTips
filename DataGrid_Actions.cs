//Current position
		int position=usersDataGridView.CurrentRow.Index;

//Load data into DataGrid using ADO.Net
 		private DataTable LoadData(DataGridView dgv)
 		{
  			DataTable aDataTable = new DataTable(); 
  			using (SqlConnection conn = new SqlConnection(connString))
  			{
     			string query = "select UserID, UserFirstName, UserSecondName, UserBdate, UserName, UserMail from Users";
     			using (SqlCommand cmd = new SqlCommand(query, conn))
     			{
         			try
        		 	{
        		     	conn.Open();
       		      	SqlDataReader reader = cmd.ExecuteReader();
      		       	aDataTable.Load(reader);
      		       	dgv.DataSource = aDataTable;
      		       	reader.Close();
    		     	}
         			catch (Exception)
     		    	{
          		   	MessageBox.Show("A problem with the User SQL Connection occurs");
            		 	throw;
       		  	}
     			}
 			}
 			return aDataTable    
		} 

//Retrieve data form a data grid to a text boxes set. (Two ways)
        private void dgvUsers_Click(object sender, EventArgs e)
        {
            //Retrieves data to text boxes directly from the Data Grid
     /*       int gridIndex = dgvUsers.CurrentRow.Index;
            txtUserID.Text = dgvUsers[0, gridIndex].Value.ToString();
            txtUserFirstName.Text = dgvUsers[1, gridIndex].Value.ToString();
            txtUserSecondName.Text = dgvUsers[2, gridIndex].Value.ToString();
            dtpUserBDate.Value = Convert.ToDateTime(dgvUsers[3, gridIndex].Value.ToString());
            txtUserName.Text = dgvUsers[4, gridIndex].Value.ToString();
            txtUserMail.Text = dgvUsers[5, gridIndex].Value.ToString();
            */
            
            //Retrieves data to text boxes usin a SQL.
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //Take the UserId to find
                int aValue = Convert.ToInt32(dgvUsers[0, dgvUsers.CurrentRow.Index].Value);
                string query = "select UserID, UserFirstName, UserSecondName, UserBdate, UserName, UserPass, UserMail from Users where UserID = @userID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int));
                    cmd.Parameters["@userID"].Value = aValue;

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            txtUserID.Text = reader.GetInt32(0).ToString();
                            txtUserFirstName.Text = reader.GetString(1);
                            txtUserSecondName.Text = reader.GetString(2);
                            dtpUserBDate.Value = Convert.ToDateTime(reader.GetDateTime(3));
                            txtUserName.Text = reader.GetString(4);
                            txtUserPass.Text = reader.GetSqlBinary(5).ToString();
                            txtUserMail.Text = reader.GetString(6);                            
                        }                        
                        reader.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("A problem with the User SQL Connection occurs");
                        throw;
                    }
                }
            }

        }

//save data from textboxes to data base using ADO.net and with a Encrypted field.
        private void tsBtnSave_Click(object sender, EventArgs e)
        {
            //Load data into data grid view: dgvUsers
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string pass = txtUserPass.Text;
                string query = String.Format(@"insert into Users(UserFirstName, UserSecondName, UserBdate, UserName, UserPass, UserMail)
                                               values(@userFirstName,@userSecondName,@userBDate,@userName,PwdEncrypt('{0}'),@userMail)",
                                               pass);

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@userFirstName", SqlDbType.VarChar));
                    cmd.Parameters["@userFirstName"].Value = txtUserFirstName.Text;

                    cmd.Parameters.Add(new SqlParameter("@userSecondName", SqlDbType.VarChar));
                    cmd.Parameters["@userSecondName"].Value = txtUserSecondName.Text;

                    cmd.Parameters.Add(new SqlParameter("@userBDate", SqlDbType.Date));
                    cmd.Parameters["@userBDate"].Value = dtpUserBDate.Value.Date;

                    cmd.Parameters.Add(new SqlParameter("@userName", SqlDbType.VarChar));
                    cmd.Parameters["@userName"].Value = txtUserName.Text;
                    
                    cmd.Parameters.Add(new SqlParameter("@userMail", SqlDbType.VarChar));
                    cmd.Parameters["@userMail"].Value = txtUserMail.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    //Update data in both data grids
                    usersTempTable = LoadData(dgvUsers);
                    this.usersTableAdapter.Fill(this.trainITDataSet.Users);
                }
            }
        }

//Verify a given password with a encrypted one into a SQL database
        private void btnLoginCheck_Click(object sender, EventArgs e)
        {
            int resultado = -1;
            if (txtUserLoginCheck.Text == "")
            {
                MessageBox.Show("Falta el usuario");
            }
            else
            {
                //Check the password.
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string passLeida = txtPassLoginCheck.Text;
                    string query = String.Format(@"select UserName from Users where UserName=@userLeido and PWdCompare('{0}',UserPass)=1",passLeida);
                    using (SqlCommand cmd = new SqlCommand(query,conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@userLeido", SqlDbType.VarChar));
                        cmd.Parameters["@userLeido"].Value = txtUserLoginCheck.Text;

                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            resultado = 50;
                        }
                        reader.Close();
                    }
                }
            }
            if (resultado > 0)
            {
                MessageBox.Show("Access Granted.");
            }
            else
            {
                MessageBox.Show("Access Denied");
            }
        }        


//Move to the previous row into a datagrid
 private void tsBtnPrevious_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow.Index > 0)
            {
                dgvUsers.CurrentCell = dgvUsers[0, dgvUsers.CurrentRow.Index - 1];
                dgvUsers_Click(sender, e);
            }
        }


//Move to the next row into a datagrid
        private void tsBtnNext_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow.Index < dgvUsers.RowCount-1)
            {
                dgvUsers.CurrentCell = dgvUsers[0, dgvUsers.CurrentRow.Index + 1];
                dgvUsers_Click(sender, e);
            }
        }


//Move to the first row into a datagrid
        private void tsBtnFirst_Click(object sender, EventArgs e)
        {
            dgvUsers.CurrentCell = dgvUsers[0, 0];
        }


//Move to the last row into a datagrid
        private void tsBtnLast_Click(object sender, EventArgs e)
        {
            dgvUsers.CurrentCell = dgvUsers[0, dgvUsers.RowCount-1];
        }
