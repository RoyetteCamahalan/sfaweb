<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="SimpleFFO.Views.MainPage" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagName="VendorComponent" Src="components/VendorComponent.ascx" TagPrefix="ucvendor"%>
<%@ Register Namespace="ListviewPaginator" Assembly="ListviewPaginator" TagPrefix="paginator" %>
<%@ Register Namespace="SimpleFFO.Content" Assembly="SimpleFFO" TagPrefix="paginator1" %>
<%@ Register TagName="SimplePaginator" Src="components/SimplePaginator.ascx" TagPrefix="ucpaginator"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Master Files ~ <%= AppModels.Pages.getPageTitle(this.myPage) %></h1>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <section class="content">
            <div class="container-fluid">
                <div class="card card-default">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Panel ID="panelfilters" runat="server" CssClass="form-group">
                                    <label>Filter By Branch:</label>
                                    <asp:DropDownList ID="cmbfilterbranch" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="cmbfilterbranch_SelectedIndexChanged">
                                        <asp:ListItem Selected="true" Value="0" Text="--All--"></asp:ListItem>
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                            <div class="col-sm-8">
                                <asp:Panel ID="panelbtncreate" runat="server" CssClass="col-lg-12 form-group">
                                    <asp:Button ID="btnCreateNew" runat="server" CssClass="btn btn-primary pull-right float-right" OnClick="btnCreateNew_Click" UseSubmitBehavior="false" Text="Create New" />
                                </asp:Panel>
                            </div>
                        </div>
                        <asp:UpdatePanel UpdateMode="Conditional" ID="upanelmaingrid" runat="server" ChildrenAsTriggers="false">
                            <ContentTemplate>

                                <% switch (this.myPage)
                                    { %>
                                <% case AppModels.Pages.pageInstitutions: %>
                                <paginator:Bootstrap ID="lstitems" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                                    <LayoutTemplate>
                                        <table class="table table-bordered table-hover dataTable dtr-inline responsive-table">
                                            <thead>
                                                <th class="text-center" style="width: 10%">Code</th>
                                                <th class="text-center">Name</th>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center"><span><%# Eval("id") %></span></td>
                                            <td><span><%# Eval("name") %></span></td>
                                        </tr>
                                    </ItemTemplate>
                                </paginator:Bootstrap>
                                
                                <paginator:Bootstrap ID="lst_institutions" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                                    <LayoutTemplate>
                                        <table class="table table-bordered table-hover dataTable dtr-inline responsive-table">
                                            <thead>
                                                <th class="text-center" style="width: 10%">Code</th>
                                                <th class="text-center">Name</th>
                                                <th class="text-center" style="width: 25%">Institution Type</th>
                                                <th class="text-center" style="width: 5%">Is Active?</th>
                                                <th class="text-center" style="width: 5%">Action</th>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center"><span><%# Eval("inst_code") %></span></td>
                                            <td class="text-center"><span><%# Eval("inst_name") %></span></td>
                                            <td class="text-center"><span><%# Eval("institutiontype.institutiontypename") %></span></td>
                                            <td class="text-center"><asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isactive") %>' Enabled="false" /></td>
                                            <td class="text-center">
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnColEditInstitution" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("inst_id")%>' CssClass="btnColEditProducts btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnColEditInstitution" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </paginator:Bootstrap>
                                <% break; %>
                                <% case AppModels.Pages.pagesRepairShops: %>
                                <asp:GridView ID="tbl_suppliers" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>

                                        <asp:TemplateField HeaderText="Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Eval("supplierno") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("suppliername") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtbranch" runat="server" Text='<%# (Eval("branch.branchname") ?? "All Branches") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="Is Active?" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isactive") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnColEditSupplier" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("supplierno")%>' CssClass="btnColEditProducts  btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnColEditSupplier" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% case AppModels.Pages.pageProducts: %>
                                <asp:GridView ID="tbl_products" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Eval("product_id") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("name") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCategory" runat="server" Text='<%# Eval("itemcategory.itemcatdescription") ?? "" %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtType" runat="server" Text='<%# AppModels.ItemTypes.getTypeName(Convert.ToInt32(Eval("itemtypeid") ?? "-1")) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnColEditProduct" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("product_id")%>' CssClass="btnColEditProducts btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnColEditProduct" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% case AppModels.Pages.pageProductCategories: %>
                                <asp:GridView ID="tbl_productcategories" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Eval("itemcatcode") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("itemcatdescription") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnColEditCategory" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("itemcatcode")%>' CssClass="btnColEditCategory btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnColEditCategory" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% case AppModels.Pages.pageVehicles: %>
                                <asp:GridView ID="tbl_vehicles" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Eval("vehicleid") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("vehiclename") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Plate #" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("platenumber") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Year - Model" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("year") + " - " + Eval("model") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Assigned To" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# getAssignedVehicle(Convert.ToInt64(Eval("vehicleid"))) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnColEditVehicle" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("vehicleid")%>' CssClass="btnColEditVehicle btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnColEditVehicle" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% case AppModels.Pages.pageEmployeeTypes: %>
                                <asp:GridView ID="tbl_employeetypes" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Eval("employeetypeid") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("employeetypedescription") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is Active?" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isactive") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btncoledit" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("employeetypeid")%>' CssClass="btnColEditVehicle btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btncoledit" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% case AppModels.Pages.pageWarehouses: %>
                                <asp:GridView ID="tbl_warehouses" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtID" runat="server" Text='<%# Eval("warehouseid") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtCode" runat="server" Text='<%# Eval("warehousecode") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("warehousedescription") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtBranch" runat="server" Text='<%# Eval("branch.branchname") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is Active?" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isactive") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btncoleditWarehouse" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("warehouseid")%>' CssClass="btnColEditVehicle btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btncoleditWarehouse" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% case AppModels.Pages.pageEmployees: %>
                               <%-- <paginator:PageList ID="lst_employees" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                                    <LayoutTemplate>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <table class="table table-bordered table-hover dataTable dtr-inline" id="table_callreportdetails">
                                                    <thead>
                                                        <th class="text-center" style="width: 10%">ID</th>
                                                        <th class="text-center">Name</th>
                                                        <th class="text-center">Type</th>
                                                        <th class="text-center">Branch</th>
                                                        <th class="text-center" style="width: 10%">Is Active?</th>
                                                        <th class="text-center" style="width: 5%">Action</th>
                                                    </thead> 
                                                    <tbody>
                                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center">
                                                <asp:Label ID="txtID" runat="server" Text='<%# Eval("employeeid") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("lastname").ToString() + ", " + Eval("firstname").ToString() %>'>
                                                </asp:Label>
                                            </td>
                                            <td class="text-center">
                                                <asp:Label ID="txtType" runat="server" Text='<%# Eval("employeetype.employeetypedescription") %>'>
                                                </asp:Label>
                                            </td>
                                            <td class="text-center">
                                                <asp:Label ID="txtBranch" runat="server" Text='<%# Eval("branch.branchname") %>'>
                                                </asp:Label>
                                            </td>
                                            <td class="text-center">
                                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isactive") %>' Enabled="false" />
                                            </td>
                                            <td class="text-center">
                                                <asp:LinkButton ID="btncoleditEmployee" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("employeeid")%>' CssClass="btnColEditVehicle btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </paginator:PageList>--%>
                                <asp:GridView ID="tbl_employees" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtID" runat="server" Text='<%# Eval("employeeid") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("lastname").ToString() + ", " + Eval("firstname").ToString() %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtType" runat="server" Text='<%# Eval("employeetype.employeetypedescription") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtBranch" runat="server" Text='<%# Eval("branch.branchname") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is Active?" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isactive") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btncoleditEmployee" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("employeeid")%>' CssClass="btnColEditVehicle btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btncoleditEmployee" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% case AppModels.Pages.pageUsers: %>
                                <asp:GridView ID="tbl_test" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtID" runat="server" Text='<%# Eval("useraccountid") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtName" runat="server" Text='<%# Eval("employee.lastname").ToString() + ", " + Eval("employee.firstname").ToString() %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtusername" runat="server" Text='<%# Eval("username") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Is Active?" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isactive") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false">
                                                    <ContentTemplate>
                                                        <div class="text-center">
                                                            <asp:LinkButton ID="btncoleditUser" runat="server" OnClick="btnColEdit_Click" CommandArgument='<% #Eval("useraccountid")%>' CssClass="btnColEditVehicle btn btn-info btn-sm"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="btncolUserPriv" runat="server" OnClick="btncolUserPriv_Click" CommandArgument='<% #Eval("useraccountid")%>' CssClass="btnColEditVehicle btn btn-danger btn-sm"><i class="fas fa-key"></i></asp:LinkButton>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btncoleditUser" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btncolUserPriv" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <% break; %>
                                <% }
%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>
    </div>
    
    <ucvendor:VendorComponent ID="ctlvendor" runat="server" OnSave="ctlvendor_Save"></ucvendor:VendorComponent>
    <button type="button" id="btnopenlibmodal" data-toggle="modal" data-target=".lib-modal" hidden="hidden"></button>
    <div class="modal fade lib-modal" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="upanelmodal" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header">
                            <h4 class="modal-title">
                                <asp:Literal ID="lblModalTitle" runat="server">New Record</asp:Literal></h4>
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <%if (this.myPage != AppModels.Pages.pageProducts && this.myPage != AppModels.Pages.pageProductCategories)
                                {%>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-check float-right">
                                        <asp:CheckBox ID="chkIsactive" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label" for="chkisactive">Is Active?</label>
                                    </div>
                                </div>
                            </div>
                            <% } %>
                            

                            <% switch (this.myPage)
                                { %>
                            <% case AppModels.Pages.pageInstitutions: %>
                            <div class="form-group">
                                <asp:Label ID="lbInstitutionName" runat="server" AssociatedControlID="txtInstitutionName">Institution Name 
                                                <span class="required">*</span></asp:Label>
                                <asp:TextBox ID="txtInstitutionName" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback">
                                    <asp:Literal ID="errtxtInstitutionName" runat="server"></asp:Literal></span>
                            </div>
                            <div class="form-group">
                                <label>Location</label>
                                <asp:TextBox ID="txtLocation" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control rounded-0"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Select Institution Type<span aria-hidden="true">*</span></label>
                                <asp:DropDownList ID="cmbIntitutionType" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                            </div>
                            <% break; %>
                            <% case AppModels.Pages.pageProducts: %>
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtProductName">Product Name <span class="required">*</span></asp:Label>
                                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback">
                                    <asp:Literal ID="errtxtProductName" runat="server"></asp:Literal></span>
                            </div>
                            <div class="form-group">
                                <label>Item Type<span aria-hidden="true">*</span></label>
                                <asp:DropDownList ID="cmbItemType" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="true" Value="-" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                            </div>
                            <div class="form-group">
                                <label>Category<span aria-hidden="true">*</span></label>
                                <asp:DropDownList ID="cmbItemCategory" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                            </div>
                            <div class="form-group">
                                <label>Packaging<span aria-hidden="true"></span></label>
                                <asp:DropDownList ID="cmbPackaging" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <asp:Label ID="Label3" runat="server" AssociatedControlID="txtPrice">Price <span class="required">*</span></asp:Label>

                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">₱
                                                </span>
                                            </div>
                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control rounded-0" TextMode="Number"></asp:TextBox>
                                            <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.oneabove %>'</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-xs-12">
                                    <div class="form-group">
                                        <asp:Label ID="Label4" runat="server" AssociatedControlID="txtPinMoney">Pin Money 
                                                <span class="required">*</span></asp:Label>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">₱
                                                </span>
                                            </div>
                                            <asp:TextBox ID="txtPinMoney" runat="server" CssClass="form-control rounded-0" TextMode="Number"></asp:TextBox>
                                            <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.oneabove %>'</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <% break; %>
                            <% case AppModels.Pages.pageProductCategories: %>
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="txtItemCatDesc">Category Name <span class="required">*</span></asp:Label>
                                <asp:TextBox ID="txtItemCatDesc" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback">
                                    <asp:Literal ID="errtxtItemCatDesc" runat="server"></asp:Literal></span>
                            </div>
                            <% break; %>
                            <% case AppModels.Pages.pageVehicles: %>
                            <div class="form-group">
                                <label for="txtvehiclename">Vehicle Name<span class="required">*</span></label>
                                <asp:TextBox ID="txtvehiclename" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtplateno">Plate #<span class="required">  *</span></label>
                                        <asp:TextBox ID="txtplateno" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                        <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtcurrentodo">Current ODO</label>
                                        <asp:TextBox ID="txtcurrentodo" runat="server" CssClass="form-control rounded-0" required="required" TextMode="Number"></asp:TextBox>
                                        <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.oneabove %>'</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtvehicleyear">Year</span></label>
                                        <asp:TextBox ID="txtvehicleyear" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                        <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtvehiclemodel">Model</label>
                                        <asp:TextBox ID="txtvehiclemodel" runat="server" CssClass="form-control rounded-0" required="required" TextMode="Number"></asp:TextBox>
                                        <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>Assigned To</label>
                                <asp:DropDownList ID="cmbAssignedTo" runat="server" CssClass="form-control select2 rounded-0 cmbAssignedTo" TabIndex="-1" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <% break; %>
                            <% case AppModels.Pages.pageEmployeeTypes: %>
                            <asp:Panel ID="paneltxtemployeetypeid" runat="server" CssClass="form-group">
                                <label for="txtemployeetypeid">Employee Type ID<span class="required">*</span></label>
                                <asp:TextBox ID="txtemployeetypeid" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                <span class="error invalid-feedback">
                                    <asp:Literal ID="errortxtemployeetypeid" runat="server"></asp:Literal></span>
                            </asp:Panel>
                            <div class="form-group">
                                <label for="txtemployeetypename">Employee Type Description<span class="required">*</span></label>
                                <asp:TextBox ID="txtemployeetypename" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                            </div>
                            <% break; %>
                            <% case AppModels.Pages.pageWarehouses: %>
                            <asp:Panel ID="paneltxtwarehouseid" runat="server" CssClass="form-group">
                                <label for="txtwarehouseid">Warehouse ID<span class="required">*</span></label>
                                <asp:TextBox ID="txtwarehouseid" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                <span class="error invalid-feedback">
                                    <asp:Literal ID="errortxtwarehouseid" runat="server"></asp:Literal></span>
                            </asp:Panel>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="cmbwarehousebranches">Branch <span class="required">*</span></label>
                                        <asp:DropDownList ID="cmbwarehousebranches" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtwarehousecode">Warehouse Code<span class="required">*</span></label>
                                        <asp:TextBox ID="txtwarehousecode" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtwarehousename">Warehouse Description<span class="required">*</span></label>
                                <asp:TextBox ID="txtwarehousename" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                            </div>
                            <% break; %>
                            <% case AppModels.Pages.pageEmployees: %>
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtemplastname">Last Name <span class="required">*</span></label>
                                        <asp:TextBox ID="txtemplastname" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtempfirstname">First Name <span class="required">*</span></label>
                                        <asp:TextBox ID="txtempfirstname" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtempmiddlename">Middle Name </label>
                                        <asp:TextBox ID="txtempmiddlename" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtempcontact">Gender </label>
                                        <asp:DropDownList ID="cmbempgender" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="true" Value="M" Text="Male"></asp:ListItem>
                                            <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtempaddress">Address</label>
                                        <asp:TextBox ID="txtempaddress" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtempcontact">Contact # </label>
                                        <asp:TextBox ID="txtempcontact" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="txtempemail">Email </label>
                                        <asp:TextBox ID="txtempemail" runat="server" CssClass="form-control rounded-0" required="required" TextMode="Email"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="cmbempbranches">Branch <span class="required">*</span></label>
                                        <asp:DropDownList ID="cmbempbranches" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="cmbempbranches_SelectedIndexChanged">
                                            <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="cmbempwarehouse">Warehouse</span></label>
                                        <asp:DropDownList ID="cmbempwarehouse" runat="server" CssClass="custom-select rounded-0" TabIndex="-1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="cmbemptype">Employee Type <span class="required">*</span></label>
                                        <asp:DropDownList ID="cmbemptype" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="cmbemptype_SelectedIndexChanged">
                                            <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="cmbempdm">Supervisor</label>
                                        <asp:DropDownList ID="cmbempdm" runat="server" CssClass="custom-select rounded-0" TabIndex="-1">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <% break; %>
                            <% case AppModels.Pages.pageUsers: %>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="form-group float-right">
                                            <asp:CheckBox ID="chkisadmin" runat="server" CssClass="form-check-input"/>
                                            <label class="form-check-label" for="chkisadmin">Is Admin?</label>
                                        </div>
                                    </div>
                                </div>
                            <asp:Panel ID="panelcmbuseremployee" runat="server" CssClass="form-group">
                                <label for="cmbuseremployee">Employee <span class="required">*</span></label>
                                <asp:DropDownList ID="cmbuseremployee" runat="server" CssClass="form-control select2 rounded-0 cmbuseremployee" TabIndex="-1" AppendDataBoundItems="true" Style="width: 100%">
                                </asp:DropDownList>
                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                            </asp:Panel>
                            <div class="form-group">
                                <label for="txtuseruname">User Name <span class="required">*</span></label>
                                <asp:TextBox ID="txtuseruname" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback">
                                    <asp:Literal ID="errtxtuseruname" runat="server"></asp:Literal>>
                            </div>
                            <div class="form-group">
                                <label for="txtuserpassword">Password </label>
                                <asp:TextBox ID="txtuserpassword" runat="server" CssClass="form-control rounded-0" required="required" TextMode="Password"></asp:TextBox>
                            </div>
                            <% break; %>
                            <% } %>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <asp:Button ID="btnCloseModalBottom" runat="server" CssClass="btn btn-default" data-dismiss="modal" UseSubmitBehavior="false" CausesValidation="false" Text="Close" />
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" UseSubmitBehavior="false"
                                CausesValidation="false" Text="Save Changes" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="cmbempbranches" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <button type="button" id="btnopenuserprivmodal" data-toggle="modal" data-target=".userpriv-modal" hidden="hidden"></button>
    <div class="modal fade userpriv-modal" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="upaneluserprivmodal" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header">
                            <h4 class="modal-title">User Privileges -
                                <asp:Literal ID="lbluserpriv" runat="server"></asp:Literal></h4>
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-check float-right">
                                        <asp:CheckBox ID="chkselectall" runat="server" CssClass="form-check-input" AutoPostBack="true" OnCheckedChanged="chkselectall_CheckedChanged"/>
                                        <label class="form-check-label" for="chkselectall">Check All</label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:GridView ID="tbl_userprivileges" runat="server"
                                        AutoGenerateColumns="false"
                                        UseAccessibleHeader="true"
                                        ShowHeaderWhenEmpty="true"
                                        CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Modules" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtID" runat="server" Text='<%# Eval("modname") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Can Request?" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkcanrequest" runat="server" Checked='<%# Eval("canrequest") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Can Add" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkcanadd" runat="server" Checked='<%# Eval("canadd") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Can Edit" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkcanedit" runat="server" Checked='<%# Eval("canedit") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="col-sm-4">
                                    <asp:GridView ID="tbl_usersubpriv" runat="server"
                                        AutoGenerateColumns="false"
                                        UseAccessibleHeader="true"
                                        ShowHeaderWhenEmpty="true"
                                        CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sub Modules" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtID" runat="server" Text='<%# Eval("submoddescription") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Can Access?" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkcanaccess" runat="server" Checked='<%# Eval("canaccess") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <asp:Button ID="btncloseuserprivmodal" runat="server" CssClass="btn btn-default" data-dismiss="modal" UseSubmitBehavior="false" CausesValidation="false" Text="Close" />
                            <asp:Button ID="btnsaveuserpriv" runat="server" CssClass="btn btn-primary" UseSubmitBehavior="false"
                                CausesValidation="false" Text="Save Changes" OnClick="btnsaveuserpriv_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnsaveuserpriv" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="chkselectall"/>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>
    <script type="text/javascript">  
        $(document).ready(function () {
            bindJs();
        });

        function bindJs() {
            $('.select2').select2({
                width: "100%",
                dropdownParent: $('.lib-modal')
            });

            $('#MainContent_tbl_suppliers').DataTable();
            if ($.fn.dataTable.isDataTable('#MainContent_tbl_institutions')) {
                $('#MainContent_tbl_products').DataTable();
            }
            else {
                $('#MainContent_tbl_institutions').DataTable({
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "autoWidth": true,
                    "responsive": true,
                });
            }
            $('#MainContent_tbl_products').DataTable();
            $('#MainContent_tbl_productcategories').DataTable();
            $('#MainContent_tbl_warehouses').DataTable();
            $('#MainContent_tbl_employees').DataTable();
            $('#MainContent_tbl_test').DataTable();
            $('#MainContent_tbl_suppliers').DataTable();
            $('.dtpDate').daterangepicker({
                singleDatePicker: true,
                constrainInput: true,
                calender_style: "picker_3"
            }, function (start, end, label) {
                $('.dtpDate').trigger('change');
            });
        };

        if ($.fn.dataTable.isDataTable('.responsive-table')) {
            $('.responsive-table').DataTable();
        } else {
            $('.responsive-table').DataTable({
                "paging": false,
                "lengthChange": false,
                "searching": false,
                "ordering": false,
                "info": false,
                "autoWidth": true,
                "responsive": true,
            });
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            bindJs()
        });
    </script>
</asp:Content>
