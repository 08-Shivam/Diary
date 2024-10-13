using System;
using System.Web.UI.WebControls;
using System.IO;
using Diary.Models;
using Newtonsoft.Json;
using System.Linq;
using Spire.Pdf.Graphics;
using Spire.Pdf;
using System.Drawing;
using System.Web;


namespace Diary
{
    public partial class Notepad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("F");

            if (Request.Cookies["email"]!=null) 
            {
                lblDetail.Text = "Welcome to the Digital Diary: " + Request.Cookies["email"].Value;
                if (!IsPostBack)
                {
                    BindFiles();
                    Clear();
                }
            }
            else if (Request.Cookies["email"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            emptyalert.Visible = false;

        }

        //loading files of saved files in a list
        public void LoadDataFromJson()
        {
            try
            {
                string jsonFilePath = Server.MapPath("~/assets/data.json");
                if (File.Exists(jsonFilePath))
                {
                    string jsonData = File.ReadAllText(jsonFilePath);
                    DataModel model = JsonConvert.DeserializeObject<DataModel>(jsonData);
                    Session["data"] = model; // Store the deserialized data into session
                }
                else
                {
                    Session["data"] = new DataModel(); // Create a new model if file doesn't exist
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error loading data: " + ex.Message + "');</script>");
            }
        }

        private void BindFiles()
        {
            // Load the data from the session
            DataModel model = (DataModel)Session["data"];
            string email = Session["email"].ToString();

            // Load user-specific files based on their email
            var files = model.Directories.Where(x => x.Email == email).ToList();

            // Refresh the data from the JSON file
            LoadDataFromJson();

            // Bind the files list to the repeater for the logged-in user
            repeatFiles.DataSource = files;
            repeatFiles.DataBind();

            // Check if the logged-in user is Admin
            string role = Session["role"] as string;
            if (role == "Admin")
            {
                // Load data from JSON again (optional, depending on how you structure your data loading)
                LoadDataFromJson();
                // Binding the user list if the user is Admin
                Repeater1.DataSource = model.Register.ToList();
                Repeater1.DataBind();
                //UsersPanelList div is visible
                UsersPanelList.Visible = true;
            }
            else
            {
                //Repeater1 hide if the user is not an Admin
                UsersPanelList.Visible = false;
            }
        }

        //Saving files Button action
        protected void btSave_Click(object sender, EventArgs e)
        {
            string rootPath = Server.MapPath("~/assets/Notepad_Diary/");

            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fileName = lblCreate.Text;     //taking filename from frontend by user
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Response.Write("<script>alert('File name cannot be empty.');</script>");
                return;
            }
            string IsFileNameExist =$"{fileName}.txt";
            string filepath = Path.Combine(rootPath, fileName);  //combine rootPath and fileName
            string b = filepath;
            string content = txtNote.Text;       //taking text data from frontend by user
            if (string.IsNullOrWhiteSpace(content))
            {
                Response.Write("<script>alert('Cannot save empty file.');</script>");
                return;
            }

            string emailId = Session["email"]?.ToString();   //Checking if session is expired
            if (string.IsNullOrEmpty(emailId))
            {
                Response.Write("<script>alert('User session expired. Please log in again.');</script>");
                Response.Redirect("Default.aspx");
                return;
            }

            // Check if a file with the name already exists 
            DataModel model1 = (DataModel)Session["data"];
            var fileExists = model1.Directories.Any(x => x.Email == emailId && string.Equals(x.Name, fileName, StringComparison.OrdinalIgnoreCase));
            if (fileExists)
            {
                try
                {
                    // Overwriting the file content
                    File.WriteAllText(b, content);

                    // Updating the existing entry in the directory model
                    lock (this)
                    {
                        var existingFile = model1.Directories.First(x => x.Email == emailId && string.Equals(x.Name, fileName, StringComparison.OrdinalIgnoreCase));
                        existingFile.Path = b; // Updating the path if necessary, though it may not change

                        // Serialize and update the data.json
                        string jsonFilePath = Server.MapPath("~/assets/data.json");
                        string jsonData = JsonConvert.SerializeObject(model1);
                        File.WriteAllText(jsonFilePath, jsonData, System.Text.Encoding.UTF8);

                        Session["data"] = model1;
                    }

                    Response.Write("<script>alert('Existing file edited successfully.');</script>");
                    Clear();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    Response.Write("<script>alert('Error occurred while overwriting the file.');</script>");
                }
            }
            else
            {
                try
                    {
                File.WriteAllText(filepath, content);

                // Use lock to prevent concurrency issues
                lock (this)
                {
                    DataModel model = (DataModel)Session["data"];
                        model.Directories.Add(new Models.Directory
                        {
                            Role = Session["role"].ToString(),
                            Id = model.Directories.Any() ? model.Directories.Max(x => x.Id) + 1 : 1,
                            Name = lblCreate.Text,
                            Path = filepath,
                            Email = Session["email"].ToString(),

                        });

                    // Serialize and update the data.json
                    string jsonFilePath = Server.MapPath("~/assets/data.json");
                    string jsonData = JsonConvert.SerializeObject(model);
                    File.WriteAllText(jsonFilePath, jsonData, System.Text.Encoding.UTF8);

                    Session["data"] = model;
                }

                BindFiles();
                Response.Write("<script>alert('File Saved Successfully as: " + fileName + "');</script>");
                Clear();
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write("<script>alert('Error occurred while saving the file.');</script>");
            }
            }
        }

        //File open action button
        protected void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                var id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                var model = (DataModel)Session["data"];
                var file = model.Directories.FirstOrDefault(x => x.Id == id && x.Email == Session["email"].ToString());

                if (file == null || !File.Exists(file.Path))
                {
                    Response.Write("<script>alert('File not found.');</script>");
                    return;
                }

                var str = File.ReadAllText(file.Path);
                var fileName = Path.GetFileNameWithoutExtension(file.Path);

                // Store file content and name in session which will used in Export_Click action button
                Session["OpenedFileContent"] = str;
                Session["OpenedFileName"] = fileName;

                txtNote.Text = str;
                lblCreate.Text = fileName;
                emptyalert.Visible = false;
                EmptyAlert();
            }
            catch (Exception ex)
            {
                LogError(ex);
                emptyalert.Text = "Error loading the file.";
                emptyalert.Visible = true;
                Clear();
            }
        }

        //Export to PDF Action button
        protected void Export_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the file content and name from session
                string content = Session["OpenedFileContent"] as string;
                string fileName = Session["OpenedFileName"] as string;

                if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(fileName))
                {
                    Response.Write("<script>alert('No file opened to export.');</script>");
                    return;
                }

                // Define the export file path (e.g., save as PDF)
                string exportFilePath = Server.MapPath("~/assets/exported_" + fileName + ".pdf");

                // Create a new PDF document using FreeSpire.PDF
                PdfDocument pdfDoc = new PdfDocument();

                // Add a page to the PDF
                PdfPageBase page = pdfDoc.Pages.Add();

                // Set up the font and layout
                PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 12);
                PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Left);

                // Add the file content as text to the PDF
                page.Canvas.DrawString(content, font, PdfBrushes.Black, new RectangleF(0, 0, page.Canvas.ClientSize.Width, page.Canvas.ClientSize.Height), format);

                // Save the document to the specified file path
                pdfDoc.SaveToFile(exportFilePath);

                // Notify the user
                Response.Write("<script>alert('File exported successfully as " + fileName + ".pdf');</script>");

                // Optionally, offer the PDF for download
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
                Response.TransmitFile(exportFilePath);
                Response.End();
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write("<script>alert('Error exporting the file to PDF.');</script>");
            }
        }

        //Delete files from list
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                var commandArgument = btn.CommandArgument;

                if (string.IsNullOrEmpty(commandArgument))
                {
                    Response.Write("<script>alert('Invalid command argument.')</script>");
                    return;
                }

                if (!int.TryParse(commandArgument, out int id))
                {
                    Response.Write("<script>alert('Invalid file ID format.')</script>");
                    return;
                }

                var model = (DataModel)Session["data"];
                var fileToDelete = model.Directories.FirstOrDefault(x => x.Id == id && x.Email == Session["email"].ToString());

                if (fileToDelete != null)
                {
                    if (File.Exists(fileToDelete.Path))
                    {
                        File.Delete(fileToDelete.Path);
                    }

                    model.Directories.Remove(fileToDelete);

                    lock (this)
                    {
                        string jsonFilePath = Server.MapPath("~/assets/data.json");
                        string jsonData = JsonConvert.SerializeObject(model);
                        File.WriteAllText(jsonFilePath, jsonData, System.Text.Encoding.UTF8);
                    }

                    Session["data"] = model;
                    BindFiles();
                    Response.Write("<script>alert('File deleted successfully.');</script>");
                }
                else
                {
                    Response.Write("<script>alert('File not found or you do not have permission.')</script>");
                }

                Clear();
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write("<script>alert('Error deleting the file.');</script>");
            }
        }

        //Delete User Action button
        protected void DeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the user is Admin
                string role = Session["role"].ToString();
                if (role != "Admin")
                {
                    Response.Write("<script>alert('You do not have permission to delete users. Only Admin can delete users.');</script>");
                    return;
                }

                var btn = (Button)sender;
                var commandArgument = btn.CommandArgument;

                if (string.IsNullOrEmpty(commandArgument))
                {
                    Response.Write("<script>alert('Invalid command argument.')</script>");
                    return;
                }

                var model = (DataModel)Session["data"];
                var userToDelete = model.Register.FirstOrDefault(x => x.Role == commandArgument);
               
                if (userToDelete != null)
                {
                    model.Register.Remove(userToDelete);

                    lock (this)
                    {
                        string jsonFilePath = Server.MapPath("~/assets/data.json");
                        string jsonData = JsonConvert.SerializeObject(model);
                        File.WriteAllText(jsonFilePath, jsonData, System.Text.Encoding.UTF8);
                    }

                    Session["data"] = model;
                    // Rebind the updated user list to the Repeater after deletion
                    BindFiles();

                    Response.Write("<script>alert('User deleted successfully.');</script>");
                }
                else
                {
                    Response.Write("<script>alert('User not found.');</script>");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                Response.Write("<script>alert('Error deleting the user.');</script>");
            }
        }

        //Logout Button
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();

            if (Request.Cookies["email"] != null)
            {
                HttpCookie emailCookie = new HttpCookie("email");
                emailCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(emailCookie); 
            }
            Response.Redirect("Default.aspx");
        }

        private void LogError(Exception ex)
        {
            // Log the error to a file or a logging service
            string logFilePath = Server.MapPath("~/assets/error.log");
            File.AppendAllText(logFilePath, DateTime.Now + ": " + ex.ToString() + Environment.NewLine);
        }

        void EmptyAlert()
        {
            emptyalert.Text = string.Empty;
        }

        private void Clear()
        {
            txtNote.Text = string.Empty;
            lblCreate.Text = string.Empty;
        }
    }
}