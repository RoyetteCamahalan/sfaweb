<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InstitutionDoctors.aspx.cs" Inherits="SimpleFFO.views.InstitutionDoctors" %>

<%@ Import Namespace="System.Web.Routing" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>



    <div class="content-wrapper">

        <section class="content">
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"></h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upanelreportmain" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <asp:Panel ID="panelrbdm" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Region/RBDM</label>
                                            <asp:DropDownList ID="cmbrbdm" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbrbdm_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelbranch" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Branch</label>
                                            <asp:DropDownList ID="cmbbranches" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbbranches_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelbbdm" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>BBDM</label>
                                            <asp:DropDownList ID="cmbbbdm" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbbbdm_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelpsr" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>PSR/PTR</label>
                                            <asp:DropDownList ID="cmbpsr" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbpsr_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="row">
                                    <asp:Panel ID="panel2" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Filter MD Type</label>
                                            <asp:DropDownList ID="cmbfiltertype" runat="server" CssClass="custom-select rounded-0"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panel1" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Filter Active</label>
                                            <asp:DropDownList ID="cmbfilteractive" runat="server" CssClass="custom-select rounded-0"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <div class="col-md-6 form-group">
                                        <div style="position: absolute; right:0; bottom:0;">
                                            <asp:Button ID="ButtonLoad" runat="server" Text=" Load Data" CssClass="btn btn-info btn-flat float-right" Style="margin-right: 5px; display: none" OnClick="btnLoadData" />
                                            <asp:Button ID="btnloadinit" runat="server" Text=" Load Data" CssClass="btn btn-info btn-flat float-right" Style="margin-right: 5px;" OnClick="btnloadinit_Click"/>
                                           
                                            <asp:Button ID="btnexportinit" runat="server" Text=" Export to Excel" CssClass="btn btn-info btn-flat float-right" Style="margin-right: 5px;" OnClick="btnexportinit_Click" />
                                            <asp:Button ID="btnexportfile" runat="server" Text=" Export to Excel" CssClass="btn btn-info btn-flat float-right" Style="margin-right: 5px; display: none" OnClick="btnGenerateExcel" />

                                            <asp:Button ID="btnexportmduinit" runat="server" Text=" Export MDU Altered " CssClass="btn btn-info btn-flat float-right" Style="margin-right: 5px;" OnClick="btnMDU_click"/>
                                            <asp:Button ID="btnexportmdufile" runat="server" Text=" Export MDU Altered " CssClass="btn btn-info btn-flat float-right" Style="margin-right: 5px; display: none" OnClick="btnGenerateMDUALT" />

                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbbranches" />
                                <asp:AsyncPostBackTrigger ControlID="cmbbbdm" />
                                <asp:AsyncPostBackTrigger ControlID="cmbpsr" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonLoad" />
                                <asp:AsyncPostBackTrigger ControlID="btnloadinit" />        
                                <asp:AsyncPostBackTrigger ControlID="btnexportinit" />
                                <asp:AsyncPostBackTrigger ControlID="btnexportmduinit" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="upanelgrids" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="panelpreviewunavailable" runat="server" CssClass="col-lg-12" Visible="false">
                                    <div class="text-center">
                                        <span>
                                            <asp:Literal ID="lblprevunavailable" runat="server"></asp:Literal></span>
                                    </div>
                                </asp:Panel>
                                <ul class="nav nav-tabs" id="tablistmenu" role="tablist" runat="server">
                                  <li class="nav-item">
                                      <asp:Panel ID="navtabdoctors" runat="server" CssClass="nav-link active">
                                    <asp:LinkButton ID="btntabdoctors" runat="server" CausesValidation="false" OnClick="btntabdoctors_Click" CommandArgument="0">Doctors</asp:LinkButton>
                                      </asp:Panel>
                                  </li>
                                  <li class="nav-item" runat="server">
                                      <asp:Panel ID="navtabpsrlist" runat="server" CssClass="nav-link">
                                        <asp:LinkButton ID="btntabpsrlist" runat="server" CausesValidation="false" OnClick="btntabdoctors_Click" CommandArgument="2">My List </asp:LinkButton>
                                      </asp:Panel>
                                  </li>
                                  <li class="nav-item" runat="server">
                                      <asp:Panel ID="navtabforapproval" runat="server" CssClass="nav-link">
                                        <asp:LinkButton ID="btntabforapproval" runat="server" CausesValidation="false" OnClick="btntabdoctors_Click" CommandArgument="1">For Approval </asp:LinkButton>
                                          <span class="badge bg-danger"><asp:Literal ID="lblforapprovalcount" runat="server"></asp:Literal></span>
                                      </asp:Panel>
                                  </li>
                                </ul>
                                 <div class="tab-content">
                                    <asp:Panel ID="paneldoctors" runat="server" Visible="true" CssClass="mt-1">
                                        <asp:ListView ID="tbl_masterlst" runat="server" ItemPlaceholderID="itemPlaceHolder1" ViewStateMode="Disabled" EnableViewState="false">
                                        <LayoutTemplate>
                                            <table class="table table-bordered table-hover dataTable dtr-inline" id="table_tbl_masterlst">
                                                <thead>
                                                    <th class="text-center" style="width: 15%">BBDM</th>
                                                    <th class="text-center" style="width: 15%">Territory</th>
                                                    <th class="text-center" style="width: 25%">Institution</th>
                                                    <th class="text-center" style="width: 20%">MD/DS</th>
                                                    <th class="text-center" style="width: 10%">Specialization</th>                                      
                                                    <th class="text-center" style="width: 10%">Class</th>
                                                    <th class="text-center" style="width: 5%">Is Active?</th>
    <%--                                                <th class="text-center" style="width: 10%">status</th>--%>

                                                </thead>
                                                <tbody>                                                 
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToInt32(Eval("istotal"))==1 ? "font600 simple-highlight" : "" %>'>
                                                <td style="vertical-align: middle;">
                                                    <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("bbdm") %>'></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("psr") %>'></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("inst_name") %>'></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("doctor") %>'></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                        <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("specialization") %>'></asp:Label>
                                                    </div>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                        <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("class") %>'></asp:Label>
                                                    </div>
                                                </td>
                                                 <td style="vertical-align: middle;">
                                                    <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                        <asp:Label ID="lbltest" runat="server" Font-Size="Small" Text='<%# Eval("isactive") %>'></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    </asp:Panel>
                                     
                                    <asp:Panel ID="panelpsrlist" runat="server" Visible="false">
                                        <asp:Panel ID="paneladddoctor" runat="server" CssClass="row" style="margin-top: 0.5rem;">
                                            <div class="col-lg-12 mb-1">
                                                <asp:Button ID="btnadddoctor" runat="server" CssClass="btn btn-sm btn-flat btn-success float-right" OnClick="btnadddoctor_Click" UseSubmitBehavior="false" Text="Create New"/>
                                            </div>
                                        </asp:Panel>
                                        <asp:ListView ID="lstpstlist" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                                        <LayoutTemplate>
                                            <table class="table table-bordered table-hover dataTable dtr-inline" id="tbl_psrlst">
                                                <thead>
                                                    <th class="text-center" style="width: 30%">Institution</th>
                                                    <th class="text-center" style="width: 30%">MD/DS</th>
                                                    <th class="text-center" style="width: 10%">Specialization</th>                                      
                                                    <th class="text-center" style="width: 10%">Class</th>
                                                    <th class="text-center" style="width: 10%">Is Active?</th>
                                                    <th class="text-center" style="width: 10%">Action</th>
    <%--                                                <th class="text-center" style="width: 10%">status</th>--%>

                                                </thead>
                                                <tbody>                                                 
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class='<%# Convert.ToInt32(Eval("istotal"))==1 ? "font600 simple-highlight" : "" %>'>
                                                <td style="vertical-align: middle;">
                                                    <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("inst_name") %>'></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("doctor") %>'></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                        <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("specialization") %>'></asp:Label>
                                                    </div>
                                                </td>
                                                <td style="vertical-align: middle;">
                                                    <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                        <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("class") %>'></asp:Label>
                                                    </div>
                                                </td>
                                                 <td style="vertical-align: middle;">
                                                    <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                        <asp:Label ID="lblstatus" runat="server" Font-Size="Small" Text='<%# Eval("isactive") %>'></asp:Label>
                                                    </div>
                                                </td>
                                                 <td style="vertical-align: middle;">
                                                    <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                        <asp:LinkButton ID="btndeletion" CssClass="btn btn-danger btn-sm" ToolTip="Request Deletion" runat="server" OnClick="btndeletion_Click" CommandArgument='<%# Eval("doc_id") %>' CausesValidation="false" Visible='<%# Convert.ToInt32(Eval("fordeletion"))==1 %>'><i class="fas fa-trash"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnreactivate" CssClass="btn btn-primary btn-sm" ToolTip="Request Activation" runat="server" OnClick="btnreactivate_Click" CommandArgument='<%# Eval("doc_id") %>' CausesValidation="false" Visible='<%# Convert.ToInt32(Eval("foractivation"))==1 %>'>Activate</asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    </asp:Panel>
                                    <asp:Panel ID="panelforapproval" runat="server" Visible="false">
                                        <asp:Panel ID="panelapprove" runat="server" CssClass="row" style="margin-top: 0.5rem;">
                                            <div class="col-lg-12">
                                                <asp:Button ID="btndisapprove" runat="server" CssClass="btn btn-small btn-danger float-right" OnClick="btndisapprove_Click" UseSubmitBehavior="false" Text="Disapprove" />
                                                <asp:Button ID="btnapprove" runat="server" CssClass="btn btn-small btn-success float-right" OnClick="btnapprove_Click" UseSubmitBehavior="false" Text="Approve" style="margin-right: 0.5rem"/>
                                            </div>
                                        </asp:Panel>
                                        <table class="table table-bordered table-hover dataTable dtr-inline" id="tbl_forapprovallst">
                                            <thead>
                                                <th class="text-center" style="width: 10%"></th>
                                                <th class="text-center" style="width: 15%">Territory</th>
                                                <th class="text-center" style="width: 20%">MD/DS</th>
                                                <th class="text-center" style="width: 10%">Specialization</th>                                      
                                                <th class="text-center" style="width: 10%">Class</th>
                                                <th class="text-center" style="width: 25%">Institution</th>
                                                <th class="text-center" style="width: 10%">Action</th>
                                            </thead>
                                            <tbody>         
                                                <asp:ListView ID="lstforapproval" runat="server" OnItemDataBound="lstforapproval_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr class="font600 simple-highlight">
                                                            <td style="vertical-align: middle;">
                                                                <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="true" OnCheckedChanged="chkselect_CheckedChanged"/>
                                                            </td>
                                                            <td colspan="6" style="vertical-align: middle;">
                                                                <asp:Label ID="lblpsr" runat="server" Text='<%# Eval("psr") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <asp:ListView ID="lstapprovaldetails" runat="server">
                                                            <ItemTemplate>                                                                
                                                                <tr>
                                                                    <td style="vertical-align: middle;">
                                                                        <asp:CheckBox ID="chkselect" runat="server"/>
                                                                    </td>
                                                                    <td style="vertical-align: middle;">
                                                                    </td>
                                                                    <td style="vertical-align: middle;">
                                                                        <asp:Label runat="server" Font-Size="Small" Text='<%# String.Format("{0}, {1}", Eval("doctor.doc_lastname"),Eval("doctor.doc_firstname")) %>'></asp:Label>
                                                                    </td>
                                                                    <td style="vertical-align: middle;">
                                                                        <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                                            <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("doctor.specialization.name") %>'></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                    <td style="vertical-align: middle;">
                                                                        <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                                            <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("doctorclass.name") %>'></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                    <td style="vertical-align: middle;">
                                                                        <asp:Label runat="server" Font-Size="Small" Text='<%# Eval("institution.inst_name") %>'></asp:Label>
                                                                    </td>
                                                                     <td style="vertical-align: middle;">
                                                                        <div style="margin-left:auto; margin-right:auto; text-align:center;">
                                                                            <asp:Label ID="lbltest" runat="server" Font-Size="Small" Text='<%# Convert.ToBoolean(Eval("doctor.isactive") ?? false) ? "Deletion" : (Eval("doctor.updated_at")!=null ? "Re-Activation" : "New") %>' ForeColor='<%# Convert.ToBoolean(Eval("doctor.isactive") ?? false) ? System.Drawing.ColorTranslator.FromHtml("#FF0000") : System.Drawing.ColorTranslator.FromHtml("#00FF00") %>'></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </div>
                                

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btntabdoctors"/>
                                <asp:AsyncPostBackTrigger ControlID="btntabforapproval"/>
                                <asp:AsyncPostBackTrigger ControlID="btnapprove"/>
                                <asp:AsyncPostBackTrigger ControlID="btndisapprove"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div id="panelloader" class="overlay" style="display: none">
                        <i class="fas fa-2x fa-sync-alt fa-spin"></i>
                    </div>
                </div>
            </div>
        </section>
    </div>

    
    <button type="button" id="btnopenlibmodal" data-toggle="modal" data-target=".lib-modal" hidden="hidden"></button>
    <div class="modal fade lib-modal" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="upanelmodal" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtdoclastname">Last Name <span class="required">*</span></label>
                                        <asp:TextBox ID="txtdoclastname" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        <span class="error invalid-feedback"><asp:Literal ID="lblerrorlastname" runat="server"></asp:Literal></span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtdocfirstname">First Name <span class="required">*</span></label>
                                        <asp:TextBox ID="txtdocfirstname" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtdocmiddlename">Middle Name </label>
                                        <asp:TextBox ID="txtdocmiddlename" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="cmbspecialization">Specialization <span class="required">*</span></label>
                                        <asp:DropDownList ID="cmbspecialization" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="cmbdocclass">Class <span class="required">*</span></label>
                                        <asp:DropDownList ID="cmbdocclass" runat="server" CssClass="custom-select rounded-0" TabIndex="-1">
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtbesttimetocall">Best Time to Call </label>
                                        <asp:TextBox ID="txtbesttimetocall" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label>Institution</label>
                                        <asp:DropDownList ID="cmbinstitution" runat="server" CssClass="form-control select2 rounded-0" TabIndex="-1" AutoPostBack="true" OnSelectedIndexChanged="cmbinstitution_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="panelinstitution" runat="server" CssClass="row" Visible="false" style="margin-left: 7.5px; margin-right: 7.5px; margin-bottom: 7.5px; border: 1px solid #ced4da;">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtinstname">Institution Name <span class="required">*</span></label>
                                        <asp:TextBox ID="txtinstname" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtinstaddress">Address <span class="required">*</span></label>
                                        <asp:TextBox ID="txtinstaddress" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="cmbinsttype">Institution Type <span class="required">*</span></label>
                                        <asp:DropDownList ID="cmbinsttype" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtroomnumber">Room # </label>
                                        <asp:TextBox ID="txtroomnumber" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtcontactno">Contact #</label>
                                        <asp:TextBox ID="txtcontactno" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtlicenseno">License #</label>
                                        <asp:TextBox ID="txtlicenseno" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
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
                <asp:AsyncPostBackTrigger ControlID="cmbinstitution"/>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    
    <button type="button" id="btnopenconfirmationmodal" data-toggle="modal" data-target=".confirmation-modal" hidden="hidden"></button>
    <div class="modal fade confirmation-modal" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="upanelconfirmationmodal" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="form-group">
                                    <asp:Literal ID="lblconfirmationmessage" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <asp:Button ID="btncloseconfirmation" runat="server" CssClass="btn btn-default" data-dismiss="modal" UseSubmitBehavior="false" CausesValidation="false" Text="Close" />
                            <asp:Button ID="btnsaveconfirmation" runat="server" CssClass="btn btn-primary" UseSubmitBehavior="false"
                                CausesValidation="false" Text="" OnClick="btnsaveconfirmation_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnsaveconfirmation" EventName="Click" />
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
            if ($.fn.dataTable.isDataTable('#table_tbl_masterlst')) {
                $('#table_tbl_masterlst').DataTable();
            } else {
                $('#table_tbl_masterlst').DataTable({
                    "paging": false,
                    "lengthChange": false,
                    "searching": true,
                    "ordering": false,
                    "info": false,
                    "autoWidth": false,
                    "responsive": true
                })
            }
            if ($.fn.dataTable.isDataTable('#tbl_psrlst')) {
                $('#tbl_psrlst').DataTable();
            } else {
                $('#tbl_psrlst').DataTable({
                    "paging": false,
                    "lengthChange": false,
                    "searching": true,
                    "ordering": false,
                    "info": false,
                    "autoWidth": false,
                    "responsive": true
                })
            }
        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            bindJs()
        });

    </script>


</asp:Content>

