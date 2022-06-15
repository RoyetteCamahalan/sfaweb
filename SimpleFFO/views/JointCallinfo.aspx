<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JointCallInfo.aspx.cs" Inherits="SimpleFFO.views.JointCallInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

    <div class="content-wrapper">

        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Joint info</h1>
                    </div>
                </div>
            </div>
        </section>

        <section class="content">
            <div class="container-fluid">


                <div class="row" id="row1" runat="server">

                    <div class="col-md-6">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Employee Info</h3>
                            </div>

                            <div class="card-body">
                                <div class="row">

                                    <div class="col-sm-7">
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

                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <label for="txtbox_fullname">Branch</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-user"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtbox_branchname" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">

                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Call Info</h3>
                            </div>

                            <div class="card-body">

                                <div class="form-group">
                                    <label for="txtbox_Institution">Date Called </label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_callDate" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">


                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="txtbox_started">Call started</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtbox_started" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="txtbox_ended">Call ended</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtbox_ended" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>


                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="txtbox_planned">Planned?</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtbox_planned" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>


                                </div>


                                <div class="form-group">
                                    <label for="txtbox_signature">Signature</label>
                                    <div class="input-group">
                                        <asp:Image runat="server" ID="imgSRC" ImageUrl="#" Style="max-width: 100%" />
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>

                <div class="row" id="row2" runat="server">

                    <div class="col-md-6">

                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Doctors</h3>
                            </div>

                            <div class="card-body">

                                <asp:GridView ID="grddoclst" runat="server" AutoGenerateColumns="false"
                                     UseAccessibleHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found"
                                     CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">

                                    <Columns>
                                        <asp:TemplateField HeaderText="Joint Visit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("others") %>' runat="server" Font-Size="Medium" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                            </div>
                        </div>

                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Joint Notes</h3>
                            </div>

                            <div class="card-body">

                               <asp:GridView ID="grdnotes" runat="server" AutoGenerateColumns="false" 
                                   UseAccessibleHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found"
                                   CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">

                                   <Columns>
                                       <asp:TemplateField HeaderText="Notes" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80%">
                                           <ItemTemplate>
                                               <asp:Label Text='<%# Eval("notes") %>' runat="server" Font-Size="Medium"/>
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                       <%--<asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                           <ItemTemplate>
                                               <asp:Label Text='<%# Eval("type") %>' runat="server" Font-Size="Medium" Font-Bold="true" />
                                           </ItemTemplate>
                                       </asp:TemplateField>--%>
                                   </Columns>

                               </asp:GridView>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-6" id="divmats" runat="server">

                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Materials</h3>
                            </div>

                            <div class="card-body">

                                <asp:GridView ID="grdmaterialst" runat="server" AutoGenerateColumns="false" 
                                   UseAccessibleHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found"
                                   CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">

                                   <Columns>
                                       <asp:TemplateField HeaderText="Handed" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100%">
                                           <ItemTemplate>
                                               <asp:Label Text='<%# Eval("name") %>' runat="server" Font-Size="Medium"/>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                   </Columns>

                               </asp:GridView>

                            </div>

                        </div>

                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Products</h3>
                            </div>

                            <div class="card-body">

                                <asp:GridView ID="grdproductlst" runat="server" AutoGenerateColumns="false"
                                    UseAccessibleHeader="true" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">

                                    <Columns>
                                        <asp:TemplateField HeaderText="Handed" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("name") %>' runat="server" Font-Size="Medium" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>

                            </div>
                        </div>

                    </div>

                </div>

                <div class="card card-primary" id="divloc" runat="server">

                    <div class="card-header">
                        <h3 class="card-title">Location</h3>
                    </div>

                    <div class="card-body">

                        <iframe id="urlframe" runat="server" height="500" frameborder="0" style="border: 0; width: 100%"></iframe>

                    </div>

                </div>

                <div class="card card-primary" id="dvrate" runat="server">
                    <div class="card-header">
                        <h3 class="card-title">Rates </h3>
                    </div>
                    <div class="card-body">
                        <asp:ListView ID="lstrating" runat="server" OnItemDataBound="lstrating_ItemDataBound">
                            <ItemTemplate>
                                <div class="card card-default">
                                     <div class="card-header">
                                        <h3 class="card-title"><%# Eval("evaluationtype.evaltypename") %></h3>
                                    </div>
                                    <div class="card-body">
                                        <asp:ListView ID="lstcallevals" runat="server">
                                            <ItemTemplate>
                                                <div class="row">
                                                    <div style="text-align: center">
                                                      <span class="fa-stack fa-1x">
                                                        <i class="far fa-star fa-stack-2x" runat="server" style='<%# Convert.ToInt32(Eval("rating"))<3 ? "color: gainsboro" : "color: yellow" %>'></i>
                                                        <strong class="fa-stack-1x fa-stack-text file-text"><%# Eval("rating") %></strong>
                                                      </span>
                                                    </div> 
                                                    <div style="display: table-cell;">
                                                        <asp:Label runat="server" Font-Bold="true" Font-Size="Small" style="vertical-align: middle;" Text='<%# Eval("evaluation.evalname") %>'></asp:Label>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="row mt-2">
                                            <span style="font-weight:600">Remarks : </span>
                                            <asp:Label CssClass="ml-2" ID="txtremarks" runat="server" Text='<%# Eval("notes") %>'></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>

                <div class="row" runat="server" id="divservice">

                    <div class="col-md-6">
                        <div class="card card-primary">
                            <div class="card card-header">
                                <h3 class="card-title">Service Info</h3>
                            </div>

                            <div class="card-body">
                                <div class="row">

                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <label for="txtbox_servicename">Service Name</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-user"></i>
                                                    </span>
                                                    <asp:TextBox ID="txtbox_servicename" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <label for="txtbox_servicedate">Date</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-calendar"></i>
                                                    </span>
                                                    <asp:TextBox ID="txtbox_servicedate" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <label for="txtbox_observation">Observation</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-user"></i>
                                                    </span>
                                                    <asp:TextBox ID="txtbox_observation" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="card card-primary">
                            <div class="card card-header">
                                <h3 class="card-title">Attendees</h3>
                            </div>

                            <div class="card-body">

                                <asp:BulletedList runat="server" ID="servicelst" Width="100%" Height="100">
                                </asp:BulletedList>

                            </div>

                        </div>
                    </div>

                </div>

            </div>
        </section>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>

    <script type="text/javascript">
        $(document).ready(function () {
            bindJs();
        });

        function bindJs() {

            //if ($.fn.dataTable.isDataTable('#MainContent_grdnotes')) {
            //    $('#MainContent_grdnotes').DataTable();
            //} else {
            //    $('#MainContent_grdnotes').DataTable({
            //        "paging": true,
            //        "lengthChange": true,
            //        "searching": true,
            //        "ordering": false,
            //        "info": true,
            //        "autoWidth": true,
            //        "responsive": true
            //    })
            //}

            //if ($.fn.dataTable.isDataTable('#MainContent_grddoclst')) {
            //    $('#MainContent_grddoclst').DataTable();
            //} else {
            //    $('#MainContent_grddoclst').DataTable({
            //        "paging": true,
            //        "lengthChange": true,
            //        "searching": true,
            //        "ordering": false,
            //        "info": true,
            //        "autoWidth": true,
            //        "responsive": true
            //    })
            //}

            //if ($.fn.dataTable.isDataTable('#MainContent_grdmaterialst')) {
            //    $('#MainContent_grdmaterialst').DataTable();
            //} else {
            //    $('#MainContent_grdmaterialst').DataTable({
            //        "paging": true,
            //        "lengthChange": true,
            //        "searching": true,
            //        "ordering": false,
            //        "info": true,
            //        "autoWidth": true,
            //        "responsive": true
            //    })
            //}

            //if ($.fn.dataTable.isDataTable('#MainContent_grdproductlst')) {
            //    $('#MainContent_grdproductlst').DataTable();
            //} else {
            //    $('#MainContent_grdproductlst').DataTable({
            //        "paging": true,
            //        "lengthChange": true,
            //        "searching": true,
            //        "ordering": false,
            //        "info": true,
            //        "autoWidth": true,
            //        "responsive": true
            //    })
            //}
        }

    </script>

</asp:Content>
