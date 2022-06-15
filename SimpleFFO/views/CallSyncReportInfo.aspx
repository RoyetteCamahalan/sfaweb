<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CallSyncReportInfo.aspx.cs" Inherits="SimpleFFO.views.CallSyncReportInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

   <%-- <div class="preloader flex-column justify-content-center align-items-center">
        <img class="animation__shake" src="../Template/img/SIMLEFFOTEXT.png" alt="SIMPLE FFO" height="98" width="399">
    </div>--%>

        <div class="content-wrapper">

            <section class="content-header">

                <div class="container-fluid">
                    <div class="row mb-2">
                        <div class="col-sm-6">
                            <h1>Call info</h1>
                        </div>
                    </div>
                </div>
            </section>

            <section class="content">
                <div class="container-fluid">

                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Sync Report Info</h3>
                        </div>

                        <div class="card-body">

                            <div class="row">

                                <div class="col-sm-2">
                                    <div class="form-group">
                                        <label for="txtbox_docCode">Employee Code</label>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><i class="fa fa-user"></i>
                                                </span>
                                            </div>
                                            <asp:TextBox ID="txtbox_employcode" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="txtbox_fullname">Employee Name</label>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><i class="fa fa-user"></i>
                                                </span>
                                            </div>
                                            <asp:TextBox ID="txtbox_fullname" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <div class="form-group">
                                        <label for="txtbox_specialization">Date</label>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><i class="fa fa-calendar"></i>
                                                </span>
                                            </div>
                                            <asp:TextBox ID="txtbox_date" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>



                            </div>
                        </div>
                    </div>

                    <div class="row">

                        <div class="col-md-6">

                            <div class="card card-primary">
                                <div class="card-header">
                                    <h3 class="card-title">First Call</h3>
                                </div>

                                <div class="card-body">

                                    <div class="form-group">
                                        <label for="txtbox_Institution">Time Start </label>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><i class="fa fa-clock"></i>
                                                </span>
                                            </div>
                                            <asp:TextBox ID="txtbox_callDate" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">

                                        <iframe id="urlframe" runat="server" height="300" frameborder="0" style="border: 0;width: 100%"></iframe>
                                    </div>

                                </div>

                            </div>
                        </div>

                        <div class="col-md-6">

                            <div class="card card-primary">
                                <div class="card-header">
                                    <h3 class="card-title">Last Call</h3>
                                </div>

                                <div class="card-body">
                                    <div class="form-group">
                                        <label for="txtbox_Institution">Time End </label>
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><i class="fa fa-clock"></i>
                                                </span>
                                            </div>
                                            <asp:TextBox ID="txtbox_callDate1" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <iframe id="urlframe1" runat="server" height="300" frameborder="0" style="border: 0;width:100%;"></iframe>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Doctor Conducted</h3>
                        </div>

                        <div class="card-body">
                            <asp:GridView ID="grdv_doclist"
                                runat="server"
                                AutoGenerateColumns="false"
                                UseAccessibleHeader="true"
                                ShowHeaderWhenEmpty="true"
                                CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%"
                                OnRowDataBound="tbl_CallReportDetails_DataBound">

                                <Columns>

                                    <asp:TemplateField HeaderText="Doctor Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" Font-Size="Medium" Text='<%# Eval("full_name") %>' NavigateUrl='<%# getCallDetailsRoute(Eval("call_id").ToString()) %>'
                                                Target="_blank" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Notes" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("notes") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Call Started" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Font-Size="Medium" Text='<%# Convert.ToDateTime(Eval("start_datetime")).ToString("hh:mm tt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Call Ended" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Font-Size="Medium" Text='<%# Convert.ToDateTime(Eval("end_datetime")).ToString("hh:mm tt") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Call Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("call_type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Signature Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("signaturetype") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Signature" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Image ID="imgsig" CssClass="imgpreview" runat="server"
                                                Style="border-width: 0px;" Width="60" Height="60" ImageUrl="#" data-toggle="modal"
                                                data-target=".imagepreviewmodal" onclick="fnshowimgpreview(this,'#imgmainpreview');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>


                            </asp:GridView>


                        </div>

                    </div>




                </div>


            </section>


        </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>
    <%: Scripts.Render("~/bundles/fnsimple") %>
</asp:Content>
