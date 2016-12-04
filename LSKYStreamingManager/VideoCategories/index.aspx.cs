using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.VideoCategories
{
    public partial class index : System.Web.UI.Page
    {
        private TableRow AddVideoCategoryTableRow(VideoCategory category, bool highlight)
        {
            TableRow returnMe = new TableRow();

            TableCell Cell_Name = new TableCell();
            Cell_Name.Text = category.Name;
            returnMe.Cells.Add(Cell_Name);

            TableCell Cell_Hidden = new TableCell();
            Cell_Hidden.Text = Helpers.boolToYesOrNoHTML(category.IsHidden);
            returnMe.Cells.Add(Cell_Hidden);

            TableCell Cell_Private = new TableCell();
            Cell_Private.Text = Helpers.boolToYesOrNoHTML(category.IsPrivate);
            returnMe.Cells.Add(Cell_Private);
            
            TableCell Cell_ID = new TableCell();
            Cell_ID.Text = category.ID;
            returnMe.Cells.Add(Cell_ID);

            TableCell Cell_Parent = new TableCell();
            if (category.HasParent())
            {
                Cell_Parent.Text = "/" + category.ParentCategory.FullName;
            }
            else
            {
                Cell_Parent.Text = "/";
            }
            returnMe.Cells.Add(Cell_Parent);

            return returnMe;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            List<VideoCategory> AllCategories = new List<VideoCategory>();

            using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
            {
                AllCategories = VideoCategory.LoadAll(connection, false);
            }

            //drpParent.Items.Clear();
            if (!IsPostBack)
            {
                drpParent.Items.Add(new ListItem(" - No Parent Category -", string.Empty));
            }
            foreach (VideoCategory cat in AllCategories.OrderBy(c => c.FullName).ToList<VideoCategory>())
            {
                tblCategories.Rows.Add(AddVideoCategoryTableRow(cat, false));

                // Add categories to the dropdown list
                if (!IsPostBack)
                {
                    ListItem newCategoryItem = new ListItem(cat.GetFullName(), cat.ID);
                    drpParent.Items.Add(newCategoryItem);
                }

            }
            
        }

        protected void btnNewCategory_Click(object sender, EventArgs e)
        {
            // Parse the new category
            string CatName = Helpers.SanitizeGeneralInputString(txtNewCategoryName.Text);
            string Parent = Helpers.SanitizeGeneralInputString(drpParent.SelectedValue);

            bool Hidden = false;
            if (chkHidden.Checked)
            {
                Hidden = true;
            }

            bool Private = false;
            if (chkPrivate.Checked)
            {
                Private = true;
            }

            if ((!string.IsNullOrEmpty(CatName)) && (CatName.Length > 2)) 
            {                                                 
                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {
                    // Create a unique ID for the category
                    string newCatID = string.Empty;
                    do
                    {
                        newCatID = Helpers.getNewID(5);
                    } while (LiveBroadcast.DoesIDExist(connection, newCatID));
                    
                    VideoCategory NewCategory = new VideoCategory(newCatID, CatName, Parent, Hidden, Private, 0);
                    VideoCategory.InsertNew(connection, NewCategory);

                    drpParent.Items.Add(new ListItem(NewCategory.Name, NewCategory.ID));
                    tblCategories.Rows.Add(AddVideoCategoryTableRow(NewCategory, false));

                }

                txtNewCategoryName.Text = "";
                chkHidden.Checked = false;
                chkPrivate.Checked = false;

            }


        }
    }
}