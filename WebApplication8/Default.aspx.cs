using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using C = System.Data.SqlClient;
using System.Linq;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Data;
using System.Threading;

namespace WebApplication8
{
    public partial class _Default : Page
    {
        int enteredPages = 1;
        public System.Web.UI.HtmlControls.HtmlGenericControl ctrl2;
private Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }

            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, id);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }
        protected void Page_Load(object sender, EventArgs e)
        { }
        protected void Page_Init(object sender, EventArgs e) {
            //Let's fill the page with 10 lines of textboxes

            for (int i = 1; i < 11; i++) {
                Control ctrl = FindControlRecursive(Page, string.Format("div{0}", i));
                if (ctrl != null) {
                    Label label = new Label();
                    label.Text = string.Format("{0}. veebileht: ", i);
                    ctrl.Controls.Add(label);
                    ctrl2 = (System.Web.UI.HtmlControls.HtmlGenericControl)ctrl;
                    TextBox tb = new TextBox();
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    tb = createTextBox(i, "Pealkiri");
                    if (i < 6) {
                        ctrl2.Controls.Add(new LiteralControl("* "));
                        addValidator(i, tb, "Pealkiri");
                    } else {
                        ctrl2.Controls.Add(new LiteralControl("&nbsp&nbsp"));
                    }
                    tb = createTextBox(i,"URL");
                    if (i < 6) {
                        ctrl2.Controls.Add(new LiteralControl("* "));
                        addValidator(i, tb, "URL");
                    } else {
                        ctrl2.Controls.Add(new LiteralControl("&nbsp&nbsp"));
                    }
                    createTextBox(i, "Kirjeldus");
                    ctrl2.Controls.Add(new LiteralControl("&nbsp&nbsp"));
                    createTextBox(i, "Omanik");
                    ctrl2.Controls.Add(new LiteralControl("&nbsp&nbsp"));
                    tb = createTextBox(i, "Kategooria");
                    if (i < 6) {
                        ctrl2.Controls.Add(new LiteralControl("* "));
                        addValidator(i, tb, "Kategooria");
                    } else {
                        ctrl2.Controls.Add(new LiteralControl("&nbsp&nbsp"));
                    }
                    DropDownList ddl = new DropDownList();
                    ddl.Items.Add(new ListItem("Punktid", "Punktid"));
                    ddl.SelectedValue = "Punktid";
                    for (int b = 1; b < 11; b++)
                    {
                        ddl.Items.Add(new ListItem(b+"", b+""));
                    }
                    ddl.ID = string.Format("page{0}Punktid", i);
                    ddl.EnableViewState = true;
                    ctrl2.Controls.Add(ddl);
                    if (i < 6) {
                        ctrl2.Controls.Add(new LiteralControl("* "));
                        addValidatorDDL(i, ddl, "Punktid");
                    } else {
                        ctrl2.Controls.Add(new LiteralControl("&nbsp&nbsp"));
                    }
                    ctrl2.Controls.Add(new LiteralControl("<br /><br />"));
                }
            }
        }
        // validate points input
        private void addValidatorDDL(int i, DropDownList ddl, string textBoxType)
        {
            RequiredFieldValidator validator = new RequiredFieldValidator();
            validator.ControlToValidate = ddl.ID;
            validator.ErrorMessage = textBoxType;
            validator.ID = "RequiredFieldValidator" + textBoxType + i;
            validator.ForeColor = System.Drawing.Color.Red;
            validator.ValidationGroup = "vgroup" + i;
            validator.Display = ValidatorDisplay.None;
            validator.InitialValue = "Punktid";
            ctrl2.Controls.Add(validator);
        }

        private TextBox createTextBox(int i, string TextBoxType)
        {
            TextBox tb = new TextBox();
            tb.EnableViewState = true;
            tb.Attributes.Add("placeholder", TextBoxType);
            tb.ID = string.Format("page{0}{1}", i,TextBoxType);
            ctrl2.Controls.Add(tb);
            return tb;
        }
        //validate all other dynamically created inputs
        private void addValidator(int i, TextBox tb, string textBoxType)
        {
            RequiredFieldValidator validator = new RequiredFieldValidator();
            validator.ControlToValidate = tb.ID;
            validator.ErrorMessage = textBoxType;
            validator.ID = "RequiredFieldValidator"+textBoxType + i;
            validator.ValidationGroup = "vgroup" + i;
            validator.Display = ValidatorDisplay.None;
            ctrl2.Controls.Add(validator);
        }
        //get sql connection info
        static string GetConnectionStringByName(string name)
{
    string returnValue = null;
    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings[name];
    if (settings != null)
        returnValue = settings.ConnectionString;

    return returnValue;
}
        //get values from textbox3w
        protected string[] GetTextBoxValues(object sender, EventArgs e)
        {
            string[] message = new string[11];
            for (int i = 1; i < 11; i++)
            {
                Control ctrl = FindControlRecursive(Page, string.Format("div{0}", i));
                foreach (TextBox textBox in ctrl.Controls.OfType<TextBox>())
                {
                    message[i] += textBox.ID + ":" + textBox.Text + "<br />";
                }
                foreach (DropDownList ddl in ctrl.Controls.OfType<DropDownList>())
                {
                    message[i] += ddl.ID + ":" + ddl.SelectedValue + "<br />";
                }
            }
            return message;
        }
        protected void submit_Click(object sender, EventArgs e)
        {
            errorBox.Visible = false;
            Page.Validate();
            if (IsValid)
                
            {
                //ToggleWebEncrypt();

                //checking how many pages are entered
                for (int i =1; i < 11; i++)
                {
                    Control ctrl = FindControlRecursive(Page, string.Format("div{0}", i));
                    TextBox tb = null;
                    tb = (TextBox) ctrl.FindControl(string.Format("page{0}URL", i));
                    if (!string.IsNullOrWhiteSpace(tb.Text)){
                        enteredPages += 1;
                    }
                }
                //check uniquenss of name and DOB combo
                var connection = new C.SqlConnection(GetConnectionStringByName("harriConnection"));
                bool isUnique = checkUniqueness(connection);
                if (isUnique)
                {
                    //enter every page to dictionary containing all pages
                    Dictionary<string, string> mapOfSinglePage = new Dictionary<string, string>();
                    Dictionary<string, Dictionary<string, string>> mapofAllPages = new Dictionary<string, Dictionary<string, string>>();

                    string[] responses = GetTextBoxValues(sender, e);
                    string[] keys = { "pealkiri", "url", "kirjeldus", "omanik", "kategooria", "punktid" };
                    for (int i = 1; i < enteredPages; i++)
                    {
                        mapOfSinglePage = new Dictionary<string, string>();
                        foreach (string key in keys)
                        {
                            mapOfSinglePage[key] = Regex.Split(responses[i], "<br />")[Array.IndexOf(keys, key)].Split(':')[1];
                        }
                            mapofAllPages[i + ". leht"] = mapOfSinglePage;
                        

                    }
                    bool containsDuplicates = checkDuplicateURLs(mapofAllPages);
                    if (!containsDuplicates)
                    {
                        bool successfulInsertionQuery = insertRows(connection, mapofAllPages);
                        if (!successfulInsertionQuery)
                        {
                            //INSERT INTO failed for some reason. Won't show specifics to user
                           errorBox.Text ="Midagi läks lehtede sisestamise päringuga valesti";
                            errorBox.Visible = true;
                        }
                    }else
                    {
                        //duplicate url
                        errorBox.Text = "Sisestasid mingi URLi mitmekordselt";
                        errorBox.Visible = true;
                    }
                   

                }
                else
                {
                    //duplicate person
                    errorBox.Text = "Selliste andmetega inimene on juba andmebaasis olemas";
                    errorBox.Visible = true;

                }
                //if no errors
                errorBox.Text = "Andmed salvestatud. Teid suunatakse kahe sekundi pärast tulemuste lehele";
                errorBox.ForeColor = System.Drawing.Color.Green;
                errorBox.Visible = true;

                Response.AddHeader("REFRESH", "5;URL=About.aspx");
            }
        }

        private bool checkDuplicateURLs(Dictionary<string, Dictionary<string, string>> mapofAllPages)
        {
            List<string> checkDuplicates = new List<string>();
            bool containsDuplicates = false;
            for (int i = 1; i < enteredPages; i++)
            {
                //check if url in iteration is contained in list of previous urls
                string currentUrl = mapofAllPages[i + ". leht"]["url"];
                if (checkDuplicates.Contains(currentUrl))
                {
                    containsDuplicates = true;
                    i = 10;
                }
                else
                {
                    checkDuplicates.Add(currentUrl);
                }
            }
            return containsDuplicates;
        }

        private bool checkUniqueness(C.SqlConnection connection)
        {
            connection.Open();
            bool success = false;
            string firstName = first.Text;
            string lastName = last.Text;
            string dob = DOB.Text;
            var command = new C.SqlCommand();
            try
            {
                //check if INSERT INTO is successful to a databes field with UNIQUE property
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"INSERT INTO Persons (Person) VALUES ('" + String.Join(";", firstName, lastName, dob) + "')";
                int a = command.ExecuteNonQuery();
                success = true;
            }
            catch
            {
                success = false;
            }
            connection.Close();
            return success;

        }

        private bool insertRows(C.SqlConnection connection, Dictionary<string,Dictionary<string,string>> mapofAllPages)
        {
            bool success = false;
            int rowsAffected;
            var command = new C.SqlCommand();
            connection.Open();
            command.Connection = connection;
            
            command.CommandType = System.Data.CommandType.Text;
            try
            {
                for (int i = 1; i < mapofAllPages.Count+1; i++)
                {
                   command.CommandText = @"INSERT INTO Pages (URL, Title, Description, Owner, Category, Points)
VALUES ('" + mapofAllPages[i + ". leht"]["url"] + "','" + mapofAllPages[i + ". leht"]["pealkiri"] + "','" + mapofAllPages[i + ". leht"]["kirjeldus"] + "','" + mapofAllPages[i + ". leht"]["omanik"] + "','" + mapofAllPages[i + ". leht"]["kategooria"] + "'," + int.Parse(mapofAllPages[i + ". leht"]["punktid"]) + ");";
                    rowsAffected = command.ExecuteNonQuery();
                }
                success = true;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                success = false;
            }
            connection.Close();
            return success;
        }

        static void ToggleWebEncrypt()
        {
            // Open the Web.config file.
            Configuration config = WebConfigurationManager.
                OpenWebConfiguration("~");

            // Get the connectionStrings section.
            ConnectionStringsSection section =
                config.GetSection("connectionStrings")
                as ConnectionStringsSection;

            // Toggle encryption.
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
            }
            else
            {
                section.SectionInformation.ProtectSection(
                    "DataProtectionConfigurationProvider");
            }

            // Save changes to the Web.config file.
            config.Save();
            
        }
        //test button to populate all fields with random numbers
        protected void populate_Click(object sender, EventArgs e)
        {
            errorBox.Visible = false;
            var random = new Random();
            for (int i = 1; i < 11; i++)
            {
                Control ctrl = FindControlRecursive(Page, string.Format("div{0}", i));
                var emptyTextBoxes = ctrl.Controls.OfType<TextBox>()
                                         .Where(txt => txt.Text.Length == 0);
                foreach (var txt in emptyTextBoxes)
                    txt.Text = random.Next(1, 1000).ToString();
               var ddl = ctrl.Controls.OfType<DropDownList>().Where(txt =>txt.SelectedValue=="Punktid");
                foreach (var list in ddl)
                {
                    list.SelectedValue = random.Next(1, 10).ToString();
                }
            }
            
            DOB.Text = "1990-01-01";
            first.Text = random.Next(1, 1000).ToString();
            last.Text = random.Next(1, 1000).ToString();
        }
        protected void valDateRange_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //respnose.write(args.Value);
            DateTime minDate = DateTime.Parse("1000-01-01");
            DateTime maxDate = DateTime.Parse("9999-01-01");
            DateTime dt;

            args.IsValid = (DateTime.TryParse(args.Value, out dt)
                            && dt <= maxDate
                            && dt >= minDate);
        }
    }

}