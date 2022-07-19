<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SFAReports.aspx.cs" Inherits="SimpleFFO.views.SFAReports" %>

<%@ Register TagName="SimplePaginator" Src="components/SimplePaginator.ascx" TagPrefix="ucpaginator"%>

<%@ Import Namespace="System.Web.Routing" %>
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
                        <h1>SFA Reports</h1>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

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
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>Report Type</label>
                                            <asp:DropDownList ID="cmbreporttypes" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbreporttypes_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <asp:Panel ID="panelyear" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Year</label>
                                            <asp:DropDownList ID="cmbyear" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbyear_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="panelcycle" runat="server" CssClass="col-sm-3">
                                        <div class="form-group">
                                            <label>Cycle</label>
                                            <asp:DropDownList ID="cmbcycle" runat="server" CssClass="custom-select rounded-0"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>
                                </div>
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
                                    <asp:Panel ID="panelspecialization" runat="server" CssClass="col-sm-3" Visible="false">
                                        <label id="lblspecialization" Visible="false">Specialization</label>
                                        <asp:DropDownList ID="cmbspecialization" runat="server" CssClass="custom-select rounded-0"></asp:DropDownList>
                                    </asp:Panel>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12 form-group">

                                        <div id="dvProgressBar" style="float: left" runat="server" visible="false">
                                            <img src="http://i.stack.imgur.com/FhHRx.gif" alt="centered image" width:100%/>
                                            Generating, please wait....
                                        </div>

                                        <asp:Button ID="ButtonLoad" runat="server" Text=" Load Data" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px; display: none" OnClick="btnLoadData" />
                                        <asp:Button ID="btnloadinit" runat="server" Text=" Load Data" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px;" OnClick="btnloadinit_Click" />
                                        <asp:Button ID="btnexportinit" runat="server" Text=" Export to Excel" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px;" OnClick="btnexportinit_Click" />
                                        <asp:Button ID="btnexportfile" runat="server" Text=" Export to Excel" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px; display: none" OnClick="btnGenerateExcel" />
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbreporttypes" />
                                <asp:AsyncPostBackTrigger ControlID="cmbbranches" />
                                <asp:AsyncPostBackTrigger ControlID="cmbbbdm" />
                                <asp:AsyncPostBackTrigger ControlID="cmbyear" />
                                <asp:AsyncPostBackTrigger ControlID="cmbpsr" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonLoad" />
                                <asp:AsyncPostBackTrigger ControlID="btnexportinit" />
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
                                
                                <ucpaginator:SimplePaginator ID="lst_callreportdetails" runat="server" ItemPlaceholderID="itemPlaceHolder1" Visible="false">
                                    <LayoutTemplate>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <table class="table table-bordered table-hover dataTable dtr-inline">
                                                    <thead>
                                                        <th class="text-center" style="width: 15%">PMR Name</th>
                                                        <th class="text-center" style="width: 15%">Doctor</th>
                                                        <th class="text-center" style="width: 10%">Actual MCP</th>
                                                        <th class="text-center" style="width: 10%">Call Time</th>
                                                        <th class="text-center" style="width: 10%">Class / Specialization</th>
                                                        <th class="text-center" style="width: 15%">Call Notes</th>
                                                        <th class="text-center" style="width: 10%">Signature</th>
                                                        <th class="text-center" style="width: 10%">Signature Type</th>
                                                        <th class="text-center" style="width: 5%">Call Type</th>
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
                                            <td>
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("pmr_name") %>'></asp:Label></td>
                                            <td><a href="#" onclick="return window.open('<%# getCallDetailsRoute(Eval("call_id").ToString()) %>', '_blank');">
                                                <%# Eval("md") %></a>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtactualmcp" runat="server" Font-Size="Small" Text='<%# Convert.ToDateTime(Eval("actualmcpdate")).ToString("MM/dd/yyy") %>'></asp:Label></td>
                                            <td class="text-center">
                                                <asp:Label ID="txtcallstart" runat="server" Font-Size="Small" Text='<%# (Convert.ToString(Eval("start_datetime") ?? "")=="" ? "No Data" : Convert.ToDateTime(Eval("start_datetime")).ToString("hh:mm tt") + "-" + (Convert.ToString(Eval("end_datetime") ?? "")=="" ? "No Data" : Convert.ToDateTime(Eval("end_datetime")).ToString("hh:mm tt"))) %>'></asp:Label>
                                            </td>
                                            <td class="text-center">
                                                <asp:Label ID="txtspecialization" runat="server" Font-Size="Small" Text='<%# "Class " + Eval("class_code").ToString() + " / " + Eval("specialization").ToString() %>'></asp:Label></td>
                                            <td>
                                                <asp:Label ID="txtcallnotes" runat="server" Font-Size="Medium" Text='<%# Eval("callnotes") %>'></asp:Label></td>
                                            <td class="text-center">
                                                <asp:Image ID="imgsig" CssClass="imgpreview" runat="server" Style="border-width: 0px;" Width="60" Height="60" ImageUrl='<%# AppModels.imageurl + Eval("imageurl").ToString() %>' data-toggle="modal" data-target=".imagepreviewmodal" onclick="fnshowimgpreview(this,'#imgmainpreview');" /></td>
                                            <td>
                                                <asp:Label ID="txtsignaturetype" runat="server" Font-Size="Small" Text='<%# Eval("signaturetype") %>'></asp:Label></td>
                                            <td>
                                                <asp:Label ID="txtcalltype" runat="server" Font-Size="Small" Text='<%# Eval("call_type") %>'></asp:Label></td>
                                        </tr>
                                    </ItemTemplate>
                                </ucpaginator:SimplePaginator>

                                <asp:GridView ID="tbl_dailycallTracker" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Record Found"
                                    CssClass="tbl_dailycallTracker" CellSpacing="0" Width="100%" Visible="false">
                                    <Columns>
                                    </Columns>
                                </asp:GridView>

                                <ucpaginator:SimplePaginator ID="lst_syncreport" runat="server" ItemPlaceholderID="itemPlaceHolder1" Visible="false">
                                    <LayoutTemplate>
                                        <table class="table table-bordered table-hover dataTable dtr-inline responsive-table">
                                            <thead>
                                                <th class="text-center">FullName</th>
                                                <th class="text-center" style="width: 10%">PMR Code</th>
                                                <th class="text-center" style="width: 10%">Date</th>
                                                <th class="text-center" style="width: 10%">First Call</th>
                                                <th class="text-center" style="width: 10%">Location</th>
                                                <th class="text-center" style="width: 10%">Last Call</th>
                                                <th class="text-center" style="width: 10%">Location</th>
                                                <th class="text-center" style="width: 10%">Call Count</th>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center"><span><%# Eval("FullName") %></span></td>
                                            <td class="text-center"><span><%# Eval("employeecode") %></span></td>
                                            <td class="text-center"><asp:HyperLink runat="server" Font-Size="Medium" Text='<%# Convert.ToDateTime(Eval("actualcalldate")).ToString("MM/dd/yyy") %>' NavigateUrl='<%# getCallSyncRoute(Eval("employeeid").ToString(),Eval("actualcalldate").ToString())%>' Target="_blank"></asp:HyperLink></td>
                                            <td class="text-center"><span><%# Eval("firstcalltime") %></span></td>
                                            <td class="text-center"><a href="#" style="font-size: small" onclick="return window.open('<%# Eval("firstcalllocation") %>', '_blank');">View Map</a></td>
                                            <td class="text-center"><span><%# Eval("lastcalltime") %></span></td>
                                            <td class="text-center"><a href="#" style="font-size: small" onclick="return window.open('<%# Eval("lastcalllocation") %>', '_blank');">View Map</a></td>
                                            <td class="text-center"><span><%# Eval("callcount") %></span></td>
                                        </tr>
                                    </ItemTemplate>
                                </ucpaginator:SimplePaginator>
                                
                                <asp:ListView ID="lst_averagecallperday" runat="server" ItemPlaceholderID="itemPlaceHolder1" Visible="false">
                                    <LayoutTemplate>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <table class="table table-bordered table-hover dataTable dtr-inline">
                                                    <thead>
                                                        <th class="text-center" style="width: 25%">District</th>
                                                        <th class="text-center" style="width: 25%">Territory</th>
                                                        <th class="text-center" style="width: 15%">Total Calls</th>
                                                        <th class="text-center" style="width: 15%">Days in Field</th>
                                                        <th class="text-center" style="width: 20%">Avg Calls per day</th>
                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Eval("istotal").ToString() == "1" ? "font600 simple-highlight" : "" %>'>
                                            <td><span><%# Eval("bbdm") %></span></td>
                                            <td><span><%# Eval("psr") %></span></td>
                                            <td class="text-center"><span><%# Eval("totalcalls") %></span></td>
                                            <td class="text-center"><span><%# Eval("noofdays") %></span></td>
                                            <td class="text-center"><span><%# Eval("avgcallperday") %></span></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:GridView ID="tbl_weeklyincidental" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Record Found"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%" Visible="false" OnRowDataBound="tbl_averagecallperday_RowDataBound">
                                    <Columns>

                                        <asp:TemplateField HeaderText="District">
                                            <ItemTemplate>
                                                <asp:Label ID="txtbbdm" runat="server" Font-Bold="true" Text='<%# Eval("bbdm") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Territory" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtpmr" runat="server" Text='<%# Eval("psr") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Week 1" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblweek1" runat="server" Text='<%# Eval("week1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Week 2" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblweek2" runat="server" Text='<%# Eval("week2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Week 3" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblweek3" runat="server" Text='<%# Eval("week3") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Week 4" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblweek4" runat="server" Text='<%# Eval("week4") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Week 5" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblweek5" runat="server" Text='<%# Eval("week5") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Week 6" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblweek6" runat="server" Text='<%# Eval("week6") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-CssClass="font600 simple-highlight">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotal" runat="server" Text='<%# Eval("total") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                    </EmptyDataTemplate>

                                </asp:GridView>
                                <asp:Panel ID="panelcallperformanceperpsr" runat="server" Visible="false">
                                    <div class="row justify-content-center">

                                        <div class="col-sm-2" runat="server" id="divcallrate">
                                            <div class="small-box bg-success">
                                                <div class="inner">
                                                    <h3 id="crateperc" runat="server">0%</sup></h3>
                                                    <p>Call Rate<small id="lblcallrate" runat="server"></small></p>
                                                </div>
                                                <div class="icon">
                                                    <i class="fas fa-chart-pie"></i>
                                                </div>
                                                <a class="small-box-footer">Selected Month</a>
                                            </div>
                                        </div>

                                        <div class="col-sm-2" runat="server" id="divcallreach">
                                            <div class="small-box bg-success">
                                                <div class="inner">
                                                    <h3 id="creachperc" runat="server">0%</h3>
                                                    <p>Call Reach<small id="lblcallreach" runat="server"></small></p>
                                                </div>
                                                <div class="icon">
                                                    <i class="fas fa-chart-pie"></i>
                                                </div>
                                                <a class="small-box-footer">Selected Month</a>
                                            </div>
                                        </div>

                                        <div class="col-sm-2" runat="server" id="divcallfrequency">
                                            <div class="small-box bg-success">
                                                <div class="inner">
                                                    <h3 id="cfreuquencyperc" runat="server">0%</h3>
                                                    <p>Call Frequency<small id="lblcallfreq" runat="server"></small></p>
                                                </div>
                                                <div class="icon">
                                                    <i class="fas fa-chart-pie"></i>
                                                </div>
                                                <a class="small-box-footer">Selected Month</a>
                                            </div>
                                        </div>
                                        <div class="col-sm-2" runat="server" id="divcallincidental">
                                            <div class="small-box bg-info">
                                                <div class="inner">
                                                    <h3 id="incidentalcount" runat="server">0</h3>
                                                    <p>Incidental Call</p>
                                                    <br />
                                                </div>
                                                <div class="icon">
                                                    <i class="fas fa-user-plus"></i>
                                                </div>
                                                <a class="small-box-footer">Selected Month</a>
                                            </div>
                                        </div>
                                        <div class="col-sm-2" runat="server" id="divdeclaremiss">
                                            <div class="small-box bg-danger">
                                                <div class="inner">
                                                    <h3 id="missedcallcount" runat="server">0</h3>
                                                    <p>Declared Missed</p>
                                                    <br />
                                                </div>
                                                <div class="icon">
                                                    <i class="fas fa-comment-slash"></i>
                                                </div>
                                                <a class="small-box-footer">Selected Month</a>
                                            </div>
                                        </div>

                                    </div>
                                    
                                    <ucpaginator:SimplePaginator ID="lst_performancereport" runat="server" ItemPlaceholderID="itemPlaceHolder1" OnLayoutCreated="lst_performancereport_LayoutCreated" pageSize="20" Visible="false">
                                        <LayoutTemplate>
                                            <table class="table table-bordered table-hover dataTable dtr-inline responsive-table">
                                                <thead>
                                                    <th class="text-center">Doctor</th>
                                                    <th class="text-center" style="width: 15%"><asp:Literal ID="lst_performancereport_m1" runat="server"></asp:Literal></th>
                                                    <th class="text-center" style="width: 15%"><asp:Literal ID="lst_performancereport_m2" runat="server"></asp:Literal></th>
                                                    <th class="text-center" style="width: 15%"><asp:Literal ID="lst_performancereport_m3" runat="server"></asp:Literal></th>
                                                    <th class="text-center" style="width: 10%">Average</th>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-center"><span><%# Eval("doc_name") %></span></td>
                                                <td class="text-center"><span><%# Eval("month1") %></span></td>
                                                <td class="text-center"><span><%# Eval("month2") %></span></td>
                                                <td class="text-center"><span><%# Eval("month3") %></span></td>
                                                <td class="text-center"><span><%# Eval("average") %></span></td>
                                            </tr>
                                        </ItemTemplate>
                                    </ucpaginator:SimplePaginator>
                                </asp:Panel>
                                <asp:ListView ID="lst_callperformance" runat="server" ItemPlaceholderID="itemPlaceHolder1">
                                    <LayoutTemplate>
                                        <table class="tblcallperformance">
                                            <thead>
                                                <th rowspan="2" class="text-center" style="width: 15%">Region</th>
                                                <th rowspan="2" class="text-center" style="width: 15%">District</th>
                                                <th rowspan="2" class="text-center" style="width: 15%">Territory</th>
                                                <th colspan="3" class="text-center">
                                                    <asp:Literal ID="lblcallperformancec1" runat="server"></asp:Literal></th>
                                                <th colspan="3" class="text-center">
                                                    <asp:Literal ID="lblcallperformancec2" runat="server"></asp:Literal></th>
                                                <th colspan="3" class="text-center">
                                                    <asp:Literal ID="lblcallperformancec3" runat="server"></asp:Literal></th>
                                                <tr>
                                                    <th class="callperformancecell">% Rate</th>
                                                    <th class="callperformancecell">% Reach</th>
                                                    <th class="callperformancecell">% Frequency</th>
                                                    <th class="callperformancecell">% Rate</th>
                                                    <th class="callperformancecell">% Reach</th>
                                                    <th class="callperformancecell">% Frequency</th>
                                                    <th class="callperformancecell">% Rate</th>
                                                    <th class="callperformancecell">% Reach</th>
                                                    <th class="callperformancecell">% Frequency</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Eval("istotal").ToString()=="0" ? "" : "font600 simple-highlight" %>'>
                                            <td class=""><%# Eval("rbdm") %></td>
                                            <td class=""><%# Eval("bbdm") %></td>
                                            <td class=""><a href="#" onclick="return window.open('<%# Eval("psrroute").ToString() %>', '_blank');">
                                                <%# Eval("psr") %></a>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callrate1")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("plancount1") ?? 0)>0 ? Eval("callcount1") + "/" + Eval("plancount1") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callreach1")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("totalmd1") ?? 0)>0 ? Eval("totalreach1") + "/" + Eval("totalmd1") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callfreq1")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("totalmd1") ?? 0)>0 ? Eval("totalfreq1") + "/" + Eval("totalmd1") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callrate2")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("plancount2") ?? 0)>0 ? Eval("callcount2") + "/" + Eval("plancount2") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callreach2")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("totalmd2") ?? 0)>0 ? Eval("totalreach2") + "/" + Eval("totalmd2") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callfreq2")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("totalmd2") ?? 0)>0 ? Eval("totalfreq2") + "/" + Eval("totalmd2") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callrate3")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("plancount3") ?? 0)>0 ? Eval("callcount3") + "/" + Eval("plancount3") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callreach3")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("totalmd3") ?? 0)>0 ? Eval("totalreach3") + "/" + Eval("totalmd3") : "" %></span>
                                            </td>
                                            <td class="callperformancecell"><%# Convert.ToDecimal(Eval("callfreq3")).ToString(AppModels.decimaltwoformat) %>
                                                <br />
                                                <span style="font-size: smaller; font-weight: 400"><%# Convert.ToDecimal(Eval("totalmd3") ?? 0)>0 ? Eval("totalfreq3") + "/" + Eval("totalmd3") : "" %></span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                
                                <asp:GridView ID="tbl_performanceperclass" runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No Record Found"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%" Visible="false" OnRowDataBound="tbl_averagecallperday_RowDataBound">
                                    <Columns>

                                        <asp:TemplateField HeaderText="Branch">
                                            <ItemTemplate>
                                                <asp:Label ID="txtbranch" runat="server" Font-Bold="true" Text='<%# Eval("branchname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Group" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="txtclass" runat="server" Font-Bold="true" Text='<%# Eval("classname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MD Universe" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmduniverse" runat="server" Text='<%# Eval("mduniverse") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MD List" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmdlist" runat="server" Text='<%# Eval("mdlist") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MD Covered" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmdcovered" runat="server" Text='<%# Eval("mdcovered") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Right Freq" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrightfreq" runat="server" Text='<%# Eval("rigthfreq") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Call Rate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderStyle-CssClass="font600 simple-highlight" ItemStyle-CssClass="font600 simple-highlight">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcallrate" runat="server" Text='<%# Eval("callrate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Call Reach" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderStyle-CssClass="font600 simple-highlight" ItemStyle-CssClass="font600 simple-highlight">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcallreach" runat="server" Text='<%# Eval("callreach") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Frequency" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderStyle-CssClass="font600 simple-highlight" ItemStyle-CssClass="font600 simple-highlight">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcallfreq" runat="server" Text='<%# Eval("callfreq") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                    </EmptyDataTemplate>

                                </asp:GridView>

                                <asp:Listview ID="tbl_jointcalls" runat="server" ItemPlaceholderID="itemPlaceHolder1" ViewStateMode="Disabled" EnableViewState="false"> 
                                    <LayoutTemplate>
                                        <table class="table table-bordered table-hover dataTable dtr-inline" id="table_jointcalls"">
                                            <thead>		
                                                <th class="text-center" style="width: 15%">District</th>
                                                <th class="text-center" style="width: 20%">Name</th>
                                                <th class="text-center" style="width: 10%">Date</th>
                                                <th class="text-center" style="width: 10%">Call Start</th>
                                                <th class="text-center" style="width: 10%">Call End</th>
                                                <th class="text-center" style="width: 10%">Observation</th>
                                                <th class="text-center" style="width: 10%">Rating</th>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>		
                                            <td>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                    <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("bbdm") %>'></asp:Label>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                <a href="#" onclick="return window.open('<%# getjointcallRoute(Eval("call_id").ToString()) %>','_blank');"> <%# Eval("name") %></a> 
                                                </div>
                                            </td>   
                                            <td>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                <asp:Label runat="server" CssClass="text-center" Font-Size="Medium" Text='<%# Convert.ToDateTime(Eval("date")).ToString("MM/dd/yyy") %>'></asp:Label>
                                                </div>
                                            </td>
                                            
                                            <td>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Convert.ToString(Eval("start_datetime") ?? "")=="" ? " " : Convert.ToDateTime(Eval("start_datetime")).ToString("hh:mm tt") %>'></asp:Label>
                                                </div>
                                            </td>

                                            <td>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Convert.ToString(Eval("end_datetime") ?? "")=="" ? " " : Convert.ToDateTime(Eval("start_datetime")).ToString("hh:mm tt") %>'></asp:Label>    
                                                </div>
                                            </td>
                                            
                                            <td>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Eval("observation") %>'></asp:Label>
                                                </div>
                                            </td>

                                            <td>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                <asp:Label runat="server" Font-Size="Medium" Text='<%# Convert.ToString(Eval("rating") ?? 0.00) == "0.00" ? " " : Convert.ToString(Eval("rating")+"%") %>'></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Listview>

                                <asp:ListView ID="tbl_cyclespecializationreport" runat="server" ItemPlaceholderID="itemplaceholder1">
                                    <LayoutTemplate>
                                        <table class="tblcallperformance w-100">
                                            <thead>
                                                <th rowspan="2" class="text-center" style="width: 10%">Year</th>
                                                <th rowspan="2" class="text-center" style="width: 25%">Specialization</th>
                                                <th rowspan="2" class="text-center" style="width: 10%">Cycle</th>
                                                <th rowspan="2" class="text-center" style="width: 10%">Total No.</th>
                                                <th colspan="4" class="text-center"> No. of Covers </th>
                                                <tr>
                                                    <th class="text-center" style="width: 10%">Face to Face</th>
                                                    <th class="text-center" style="width: 10%">Online</th>
                                                    <th class="text-center" style="width: 10%">Total</th>
                                                    <th class="text-center" style="width: 15%">Reach</th>
                                                </tr>
                                                </thead>
                                            <tbody>
                                                <asp:PlaceHolder runat="server" ID="itemplaceholder1"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Eval("istotal").ToString()=="0" ? "" : "font600 simple-highlight" %>'>
                                            <td class="text-center"><%# Eval("cycleyear") %></td>
                                            <td class="text-center"><%# Eval("name") %></td>
                                            <td class="text-center"><%# Convert.ToString(Eval("cyclemonth") ?? "") %></td>
                                            <td class="text-center"><%# Eval("totalconduct") %></td>
                                            
                                            <td class="text-center"><%# Eval("faceToface") %></td>
                                            <td class="text-center"><%# Eval("online") %></td>
                                            <td class="text-center"><%# Eval("total") %></td>
                                            <td class="text-center"><%# Convert.ToDecimal(Eval("reach")).ToString("0.##") + "%" %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>


                                <asp:Panel ID="panelreceivingperpsr" runat="server" Visible="false">
                                    
                                    <ucpaginator:SimplePaginator ID="lst_receiving" runat="server" ItemPlaceholderID="itemPlaceHolder1" Visible="false">
                                        <LayoutTemplate>
                                            <table class="table table-bordered table-hover dataTable dtr-inline responsive-table">
                                                <thead>
                                                    <th class="text-center" style="width: 15%">Name</th>
                                                    <th class="text-center" style="width: 15%">Product Name</th>
                                                    <th class="text-center" style="width: 10%">Sample</th>
                                                    <th class="text-center" style="width: 10%">Literature</th>
                                                    <th class="text-center" style="width: 10%">promaterials</th>
                                                    <th class="text-center" style="width: 15%">date</th>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder runat="server" ID="itemplaceholder1"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-center"><span><%# Eval("name") %></span></td>
                                                <td class="text-center"><span><%# Eval("product_name") %></span></td>
                                                <td class="text-center"><span><%# Eval("sample") %></span></td>
                                                <td class="text-center"><span><%# Eval("literature") %></span></td>
                                                <td class="text-center"><span><%# Eval("promaterials") %></span></td>
                                                <td class="text-center"><span><%# Convert.ToDateTime( Eval("date")).ToString("MM/dd/yyy") %></span></td>
                                            </tr>
                                        </ItemTemplate>
                                    </ucpaginator:SimplePaginator>

                                </asp:Panel>

                                <asp:Panel ID="panelcallmaterialsperpsr" runat="server" Visible="false">

                                    <ucpaginator:SimplePaginator ID="lst_callmaterials" runat="server" ItemPlaceholderID="itemPlaceHolder1"
                                         OnLayoutCreated="lst_callmaterials_layoutCreated" pageSize="20" Visible="false">
                                        <LayoutTemplate>
                                            <table class="table table-bordered table-hover dataTable dtr-inline responsive-table">
                                                <thead>
                                                    <th class="text-center" style="width: 15%">psr</th>
                                                    <th class="text-center" style="width: 15%"> Doctor Name</th>
                                                    <th class="text-center" style="width: 15%">Product Name</th>
                                                    <th class="text-center" style="width: 10%">Sample</th>
                                                    <th class="text-center" style="width: 10%">Literature</th>
                                                    <th class="text-center" style="width: 10%">Pro Materials</th>
                                                    <th class="text-center" style="width: 10%">Date</th>
                                                    <th class="text-center" style="width: 15%">Institution</th>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-center" style="width: 15%"><span><%# Eval("psr") %></span></td>
                                                <td class="text-center" style="width: 15%"><span><%# Eval("docname") %></span></td>
                                                <td class="text-center" style="width: 15%"><span><%# Eval("name") %></span></td>
                                                <td class="text-center" style="width: 10%"><span><%# Eval("sample") %></span></td>
                                                <td class="text-center" style="width: 10%"><span><%# Eval("literature") %></span></td>
                                                <td class="text-center" style="width: 10%"><span><%# Eval("promaterials") %></span></td>
                                                <td class="text-center" style="width: 10%"><span><%# Convert.ToDateTime( Eval("date")).ToString("MM/dd/yyy") %></span></td>
                                                <td class="text-center" style="width: 15%"><span><%# Eval("inst_name") %></span></td>
                                            </tr>
                                        </ItemTemplate>
                                    </ucpaginator:SimplePaginator>
                                </asp:Panel>



                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="panelloader" class="overlay" style="display: none">
                        <i class="fas fa-2x fa-sync-alt fa-spin"></i>
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

            if ($.fn.dataTable.isDataTable('#MainContent_tbl_syncreport')) {
                $('#MainContent_tbl_syncreport').DataTable();
            } else {
                $('#MainContent_tbl_syncreport').DataTable({
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": false,
                    "info": true,
                    "autoWidth": true,
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