using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingCore.ExtensionMethods;

namespace LSKYStreamingManager.VideoCategories
{
    public partial class index : System.Web.UI.Page
    {
        private TableRow AddVideoCategoryTableRow(VideoCategory category, bool highlight)
        {
            TableRow returnMe = new TableRow();

            string categoryName = string.Empty;

            for (int x = 1; x < category.MenuLevel; x++)
            {
                categoryName += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }

            categoryName += category.Name;

            returnMe.Cells.Add(new TableCell() { Text = categoryName });
            returnMe.Cells.Add(new TableCell() { Text = category.IsHidden.ToYesOrNoHTML() });
            returnMe.Cells.Add(new TableCell() { Text = category.IsPrivate.ToYesOrNoHTML() });
            returnMe.Cells.Add(new TableCell() { Text = category.ID });
            returnMe.Cells.Add(new TableCell() { Text = category.ParentCategoryID });
            returnMe.Cells.Add(new TableCell() { Text = category.MenuLevel.ToString() });
            returnMe.Cells.Add(new TableCell() { Text = category.VideoCount.ToString() });

            return returnMe;
        }

        private void refreshCategoryList()
        {

            VideoCategoryRepository videoCategoryRepo = new VideoCategoryRepository();
            List<VideoCategory> AllCategories = videoCategoryRepo.GetAll();

            tblCategories.Rows.Clear();
            drpParent.Items.Clear();
            drpParent.Items.Add(new ListItem(" - No Parent Category -", string.Empty));

            foreach (VideoCategory cat in AllCategories.OrderBy(c => c.FullName).ToList<VideoCategory>())
            {
                tblCategories.Rows.Add(AddVideoCategoryTableRow(cat, false));

                // Add categories to the dropdown list
                if (!IsPostBack)
                {
                    ListItem newCategoryItem = new ListItem(cat.FullName, cat.ID);
                    drpParent.Items.Add(newCategoryItem);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            VideoCategoryRepository videoCategoryRepo = new VideoCategoryRepository();
            List<VideoCategory> AllCategories = videoCategoryRepo.GetAll();

            if (!IsPostBack)
            {
                refreshCategoryList();
            }
        }

        protected void btnNewCategory_Click(object sender, EventArgs e)
        {
            // Parse the new category
            string CatName = Sanitizers.SanitizeGeneralInputString(txtNewCategoryName.Text);
            string Parent = Sanitizers.SanitizeGeneralInputString(drpParent.SelectedValue);

            bool Hidden = chkHidden.Checked;

            bool Private = chkPrivate.Checked;

            if ((!string.IsNullOrEmpty(CatName)) && (CatName.Length > 2)) 
            {       
                VideoCategory NewCategory = new VideoCategory()
                {
                    Name = CatName,
                    ParentCategoryID = Parent,
                    IsHidden = Hidden,
                    IsPrivate = Private
                };
                    
                VideoCategoryRepository videoCategoryRepository = new VideoCategoryRepository();
                videoCategoryRepository.Insert(NewCategory);
                
                txtNewCategoryName.Text = "";
                chkHidden.Checked = false;
                chkPrivate.Checked = false;
                refreshCategoryList();
            }



        }
    }
}