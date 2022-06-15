using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SimpleFFO.Content
{
    public class Bootstrap1 : ListView
    {
        #region "Vars"
        public int pageSize {
            get => pagesize;
            set => pagesize = value;
        }
        #endregion
        private int pagesize = 10;
        private object Bindable { get; set; }
        private int dataSize { get; set; }
        private int totalPageCount { get; set; }
        private int pageIndex { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateLayout();
        }
        public void Bind(object bindable)
        {
            this.Bindable = bindable;
            this.Bind();
        }
        
        private void Bind()
        {
            this.DataSource = Paginate();
            this.DataBind();
            this.renderPages(dataSize);
            this.SaveViewState();
            this.SaveControlState();
        }

        public object Paginate()
        {
            object bindable = this.Bindable;

            if (bindable.GetType() == typeof(DataTable))
                this.dataSize = ((DataTable)bindable).Rows.Count;
            else if (bindable.GetType().IsGenericType && bindable is IEnumerable)
            {
                this.dataSize = ((IEnumerable)bindable).Cast<object>().Count();
            }

            this.totalPageCount = (dataSize / this.pageSize) + ((dataSize % this.pageSize) > 0 ? 1 : 0);
            
            //panelpagecontainer.Visible = this.totalPageCount > 1;
            //this.renderPages(dataSize);
            //upanelpagination.Visible = true;
            //upanelpagination.Update();

            if (dataSize < this.pageSize)
                return bindable;

            if (bindable.GetType() == typeof(DataTable))
                return ((DataTable)bindable).AsEnumerable().Skip(this.pageSize * this.pageIndex).Take(this.pageSize).CopyToDataTable();
            else if (bindable.GetType().IsGenericType && bindable is IEnumerable)
                return ((IEnumerable)bindable).Cast<object>().Skip(this.pageSize * this.pageIndex).Take(this.pageSize).ToList();

            return bindable;
        }
        private void renderPages(int dataSize)
        {
            UpdatePanel upanelpages = (UpdatePanel)this.FindControl("upanelpages");
            if (upanelpages == null)
            {
                this.CreateLayout();
                upanelpages = (UpdatePanel)this.FindControl("upanelpages");
            }
            if (upanelpages != null)
            {
                Literal lblpaginationinfo = (Literal)this.FindControl("lblpaginationinfo");

                if (dataSize > 0)
                {
                    int lastrecord = (this.pageSize * this.pageIndex) + this.pageSize;
                    lblpaginationinfo.Text = String.Format("Showing {0} to {1} of {2} entries", (this.pageSize * this.pageIndex) + 1, lastrecord > dataSize ? dataSize : lastrecord, dataSize);
                }

                HtmlGenericControl paging_simple_next = (HtmlGenericControl)this.FindControl("paging_simple_next");
                HtmlGenericControl paging_simple_previous = (HtmlGenericControl)this.FindControl("paging_simple_previous");
                ListView lstpages = (ListView)this.FindControl("simple_plst_page");


                List<object> pagenumbers = new List<object>();

                int pageNumber = 1;

                // Get Left Pointer to generate ...
                int leftPointer = this.pageIndex - 1;
                if (this.pageIndex == (this.totalPageCount - 1))
                    leftPointer = this.totalPageCount - 3;

                // Get Right Pointer to generate ...
                int rightPointer = this.pageIndex + 3;
                if (this.pageIndex == 0)
                    rightPointer = 4;

                if (rightPointer == (this.totalPageCount - 1))
                    rightPointer = this.totalPageCount;

                while (pageNumber <= totalPageCount)
                {
                    if (pageNumber == 1 ||
                        (pageNumber > leftPointer && pageNumber < rightPointer) ||
                        (pageNumber == 3 && this.pageIndex == 0) ||
                        pageNumber == this.totalPageCount)
                    {
                        pagenumbers.Add(new { pageNumber = pageNumber - 1, isactive = pageIndex == (pageNumber - 1), data = pageNumber.ToString() });
                    }
                    else if ((pageNumber == leftPointer && pageNumber != 2) ||
                             (pageNumber == rightPointer && pageNumber != (this.totalPageCount - 1)))
                    {
                        pagenumbers.Add(new { pageNumber = -3, isactive = false, data = "..." });
                    }
                    pageNumber++;
                }
                if (this.totalPageCount != (this.pageIndex + 1))
                    paging_simple_next.Attributes["class"] = paging_simple_next.Attributes["class"].Replace(" disabled", "");
                else
                    paging_simple_next.Attributes["class"] = paging_simple_next.Attributes["class"] + " disabled";

                if (this.pageIndex != 0)
                    paging_simple_previous.Attributes["class"] = paging_simple_previous.Attributes["class"].Replace(" disabled", "");
                else
                    paging_simple_previous.Attributes["class"] = paging_simple_previous.Attributes["class"] + " disabled";

                lstpages.DataSource = pagenumbers;
                lstpages.DataBind();
                upanelpages.Update();
            }
        }
        protected void lst_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if(e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlGenericControl licontrol = (HtmlGenericControl)e.Item.FindControl("pageli");
                if(licontrol != null)
                {
                    LinkButton btn = new LinkButton() { ID = "btnsimplepage", CssClass = "page-link" };
                    btn.Text = DataBinder.Eval(e.Item.DataItem, "data").ToString();
                    btn.CommandArgument= DataBinder.Eval(e.Item.DataItem, "pageNumber").ToString();
                    btn.Click += btnsimplepagenumber_Click;
                    //btn.Attributes.Add("Click", "btnsimplepagenumber_Click");
                    licontrol.Controls.Add(btn);
                    if (Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "isactive")))
                    {
                        licontrol.Attributes["class"] = licontrol.Attributes["class"] + " active";
                    }
                }
            }
        }
        protected void btnprevious_Click(object sender, EventArgs e)
        {
            if (pageIndex > 1)
            {
                pageIndex--;
                this.Bind();
            }
        }

        protected void btnsimplepagenumber_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            if (btn.CommandArgument != "-3")
            {
                this.pageIndex = Convert.ToInt32(btn.CommandArgument);
                this.Bind();
            }
        }

        protected void btnnext_Click(object sender, EventArgs e)
        {
            if (pageIndex < totalPageCount)
            {
                pageIndex++;
                this.Bind();
            }
        }
        public void CreateLayout()
        {
            UpdatePanel upanelpages = (UpdatePanel)this.FindControl("upanelpages");
            if (upanelpages == null)
            {
                upanelpages = new UpdatePanel { ID = "upanelpages", UpdateMode = UpdatePanelUpdateMode.Conditional };
                var maindiv = new HtmlGenericControl("div");
                maindiv.Attributes["class"] = "row";

                var divpaginationinfo = new HtmlGenericControl("div");
                divpaginationinfo.Attributes["class"] = "col-sm-12 col-md-5";

                var divpaginationinfoinner = new HtmlGenericControl("div");
                divpaginationinfoinner.Attributes["class"] = "dataTables_info";
                divpaginationinfoinner.Attributes["role"] = "status";

                var lblpaginationinfo = new Literal { ID = "lblpaginationinfo" };
                divpaginationinfoinner.Controls.Add(lblpaginationinfo);
                divpaginationinfo.Controls.Add(divpaginationinfoinner);

                var divpagescontainer = new HtmlGenericControl("div");
                divpagescontainer.Attributes["class"] = "col-sm-12 col-md-7";

                var divpagescontainerinner = new HtmlGenericControl("div");
                divpagescontainerinner.Attributes["class"] = "dataTables_paginate paging_simple_numbers";

                var ulpagescontainer = new HtmlGenericControl("ul");
                ulpagescontainer.Attributes["class"] = "pagination";

                var liprevious = new HtmlGenericControl("li") { ID = "paging_simple_previous" };
                liprevious.Attributes["class"] = "paginate_button page-item previous disabled";
                LinkButton btnprevious = new LinkButton() { Text = "Previous", CssClass = "page-link" };
                btnprevious.Click += new EventHandler(btnprevious_Click);

                var lstpages = new ListView() { ID = "simple_plst_page" };
                //lstpages.ItemTemplate = new IPageTemplate(ListItemType.Item);
                lstpages.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lst_ItemDataBound);

                var linext = new HtmlGenericControl("li") { ID = "paging_simple_next" };
                linext.Attributes["class"] = "paginate_button page-item next disabled";
                LinkButton btnnext = new LinkButton() { ID= "btn_paging_simple_next", Text = "Next", CssClass = "page-link", CausesValidation=false, CommandArgument="-2" };
                btnnext.Click += new EventHandler(btnnext_Click);

                liprevious.Controls.Add(btnprevious);
                //linext.Controls.Add(new LiteralControl(""));
                linext.Controls.Add(btnnext);
                ulpagescontainer.Controls.Add(liprevious);
                ulpagescontainer.Controls.Add(lstpages);
                ulpagescontainer.Controls.Add(linext);
                divpagescontainerinner.Controls.Add(ulpagescontainer);
                divpagescontainer.Controls.Add(divpagescontainerinner);

                maindiv.Controls.Add(divpaginationinfo);
                maindiv.Controls.Add(divpagescontainer);
                upanelpages.ContentTemplateContainer.Controls.Add(maindiv);
                if (LayoutTemplate == null)
                    this.CreateLayoutTemplate();

                //upanelpages.Triggers.Add(new AsyncPostBackTrigger(){ ControlID=btnnext.ID });
                this.Controls.Add(upanelpages);
                LinkButton btntest = new LinkButton() { ID = "btntest", CssClass = "page-link", Text="btntest" , CausesValidation=false };
                btntest.Click += new EventHandler(btnnext_Click);
                this.Controls.Add(btntest);

            }
        }
    }
}