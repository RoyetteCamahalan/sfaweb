<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FundingComponent.ascx.cs" Inherits="SimpleFFO.views.components.FundingComponent" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>
<%@ Import Namespace="SimpleFFO.Model" %>
<asp:Panel ID="panelfundrequest" runat="server" CssClass="card card-info panelfundrequest" Visible="false">
                <div class="card-header">
                    <h3 class="card-title">Fund Request</h3>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="upanelfundrequestdashboard" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="panelrequestheader" runat="server" CssClass="row">
                                <div class="col-sm-4">
                                    <div class="small-box bg-success">
                                        <div class="inner">
                                            <h4>₱
                                                    <asp:Literal ID="lbltotalfundreleased" runat="server"></asp:Literal></h4>

                                            Total Released
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">

                                    <div class="small-box bg-info">
                                        <div class="inner">
                                            <h4>₱
                                                    <asp:Literal ID="lbltotalfundrequested" runat="server"></asp:Literal></h4>

                                            Total Requested
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">

                                    <div class="small-box bg-primary">
                                        <div class="inner">
                                            <h4>₱
                                                    <asp:Literal ID="lblfundbudget" runat="server"></asp:Literal></h4>

                                            For Fund Request
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:GridView ID="dgvfundrequest" runat="server"
                                AutoGenerateColumns="false"
                                UseAccessibleHeader="true"
                                ShowHeaderWhenEmpty="true"
                                CssClass="table table-bordered table-hover dataTable dtr-inline" OnRowDataBound="dgvfund_RowDataBound">
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Date Need" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="txtdateneeded" runat="server" Text='<%# Convert.ToDateTime(Eval("dateneeded")).ToString(AppModels.dateformat) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Mode" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="txtpaymentmode" runat="server" Text='<%# AppModels.Funding.PaymentMode.Description(Convert.ToInt32(Eval("paymentmode"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payee">
                                        <ItemTemplate>
                                            <asp:Label ID="txtpayee" runat="server" Text='<%# Eval("paymentref") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="txtamount" runat="server" Text='<%# Convert.ToDecimal(Eval("amount")).ToString(AppModels.moneyformat) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="txtStatus" runat="server" CssClass='<%# getRequestStatusBadge((int)Eval("status")) %>' Text='<%# getRequestStatus((int)Eval("status")) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <div class="text-center">
                                                <asp:LinkButton ID="btnshowdisbursement" CssClass="btn btn-info btn-xs btn-flat" runat="server" CausesValidation="false" CommandArgument='<%# Eval("fundrequestid") %>' OnClick="btnshowdisbursement_Click"> Details </asp:LinkButton>
                                                <asp:LinkButton ID="btnroutingfundreq" CssClass="btn btn-info btn-xs btn-flat" runat="server" CausesValidation="false" CommandArgument='<%# Eval("fundrequestid") %>' OnClick="btnroutingfund_Click"> Routing </asp:LinkButton>
                                                <asp:LinkButton ID="btnapprovefundreq" CssClass="btn btn-success btn-xs btn-flat" Visible="false" runat="server" CausesValidation="false" CommandArgument='<%# Eval("fundrequestid") %>' OnClick="btnapprovefund_Click"> Approve </asp:LinkButton>
                                                <asp:LinkButton ID="btndeclinefundreq" CssClass="btn btn-danger btn-xs btn-flat" Visible="false" runat="server" CausesValidation="false" CommandArgument='<%# Eval("fundrequestid") %>' OnClick="btndeclinefund_Click"> Reject </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:Button ID="btnaddfundrequest" runat="server" Text="Add New" CssClass="btn btn-primary btn-flat float-right" OnClick="btnaddfundrequest_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <asp:UpdatePanel ID="upanelfundrequest" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="panelfundrequestentry" runat="server" CssClass="card card-primary" Visible="false">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>Date Needed</label>

                                                <div class="input-group date" id="dateneeded" data-target-input="nearest">
                                                    <asp:TextBox ID="txtdateneeded" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpDate" aria-describedby="inputSuccess2Status3"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                    <span id="spantxtdateneeded" class="sr-only">(success)</span>
                                                    <span class="error invalid-feedback"><%= AppModels.ErrorMessage.invaliddate %></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Payment Mode</label>
                                                <asp:DropDownList ID="cmbpaymentmode" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true">
                                                    <asp:ListItem Value="-1">--Select Mode--</asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Payee</label>
                                                <asp:TextBox ID="txtpayee" runat="server" CssClass="form-control rounded-0">
                                                </asp:TextBox>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>Amount</label>
                                                <asp:TextBox ID="txtfundrequestamount" runat="server" CssClass="form-control rounded-0 text-right">
                                                </asp:TextBox>
                                                <span class="error invalid-feedback">
                                                    <asp:Literal ID="errortxtfundrequestamount" runat="server"></asp:Literal></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 form-group">
                                            <asp:LinkButton ID="btncancelfundrequest" runat="server" CssClass="btn btn-default btn-sm btn-flat float-right" CausesValidation="false" OnClick="btncancelfundrequest_Click">Cancel</asp:LinkButton>
                                            <asp:LinkButton ID="btnsavefundrequest" runat="server" CssClass="btn btn-success btn-sm btn-flat float-right" Style="margin-right: 5px;" CausesValidation="false" OnClick="btnsavefundrequest_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.card-body -->
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btncancelfundrequest" />
                            <asp:AsyncPostBackTrigger ControlID="btnsavefundrequest" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

            </asp:Panel>

            <asp:Panel ID="panelfundliquidation" runat="server" CssClass="card card-info panelfundliquidation" Visible="false">
                <div class="card-header">
                    <h3 class="card-title">Fund Liquidation</h3>
                    <h3 class="card-title">Fund Liquidation</h3>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="upanelfunliquidationdashboard" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="small-box bg-success">
                                        <div class="inner">
                                            <h4>₱
                                                    <asp:Literal ID="lbltotalliquidated" runat="server"></asp:Literal></h4>
                                            Total Liquidated
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">

                                    <div class="small-box bg-danger">
                                        <div class="inner">
                                            <h4>₱
                                                    <asp:Literal ID="lblforliquidation" runat="server"></asp:Literal></h4>
                                            For Liquidation
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:GridView ID="dgvfundliquidation" runat="server"
                                AutoGenerateColumns="false"
                                UseAccessibleHeader="true"
                                ShowHeaderWhenEmpty="true"
                                CssClass="table table-bordered table-hover dataTable dtr-inline" OnRowDataBound="dgvfund_RowDataBound">
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Date Submitted" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldatesubmitted" runat="server" Text='<%# Convert.ToDateTime(Eval("datesubmitted")).ToString(AppModels.dateformat) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblactualdate" runat="server" Text='<%# Convert.ToDateTime(Eval("actualdate")).ToString(AppModels.dateformat) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="txtamount" runat="server" Text='<%# Convert.ToDecimal(Eval("amount")).ToString(AppModels.moneyformat) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Receipt Image" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <div class="row" style="text-align: center">
                                                    <asp:Image ID="imgpreview" runat="server" Height="50" Width="50" ImageUrl="#" Style="border-width: 0px; margin: auto;" Visible="false" CssClass="imgpreview" data-toggle="modal" data-target=".imagepreviewmodal" />
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="txtStatus" runat="server" CssClass='<%# getRequestStatusBadge((int)Eval("status")) %>' Text='<%# getRequestStatus((int)Eval("status")) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <div class="text-center">
                                                <asp:LinkButton ID="btnroutingfundliq" CssClass="btn btn-info btn-xs btn-flat" runat="server" CausesValidation="false" CommandArgument='<%# Eval("fundliquidationid") %>' OnClick="btnroutingfund_Click"> Routing </asp:LinkButton>
                                                <asp:LinkButton ID="btnapprovefundliq" CssClass="btn btn-success btn-xs btn-flat" Visible="false" runat="server" CausesValidation="false" CommandArgument='<%# Eval("fundliquidationid") %>' OnClick="btnapprovefund_Click"> Approve </asp:LinkButton>
                                                <asp:LinkButton ID="btndeclinefundliq" CssClass="btn btn-danger btn-xs btn-flat" Visible="false" runat="server" CausesValidation="false" CommandArgument='<%# Eval("fundliquidationid") %>' OnClick="btndeclinefund_Click"> Reject </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:Button ID="btnaddfundliquidation" runat="server" Text="Add New" CssClass="btn btn-primary btn-flat float-right" OnClick="btnaddfundliquidation_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <asp:UpdatePanel ID="upanelfundliquidation" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUpload" />
                            <asp:AsyncPostBackTrigger ControlID="btncancelfundliquidation" />
                            <asp:AsyncPostBackTrigger ControlID="btnsavefundliquidation" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel ID="panelfundliqidationentry" runat="server" CssClass="card card-primary mt-1" Visible="false">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <label>Vendor <span class="required">*</span></label>
                                            <div class="input-group">
                                                <asp:DropDownList ID="cmbvendors" runat="server" CssClass="form-control select2 rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <span class="input-group-append">
                                                    <button type="button" class="btn btn-info btn-flat"><i class="fas fa-plus fa-fw"></i></button>
                                                </span>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Remarks</label>
                                                <asp:TextBox ID="txtfundliquidationremarks" runat="server" CssClass="form-control rounded-0">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Date Submitted</label>

                                                <div class="input-group date" data-target-input="nearest">
                                                    <asp:TextBox ID="txtdatesubmitted" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0" aria-describedby="inputSuccess2Status3" ReadOnly="true"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                    <span id="spantxtdatesubmitted" class="sr-only">(success)</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Actual Date <span class="required">*</span></label>

                                                <div class="input-group date" data-target-input="nearest">
                                                    <asp:TextBox ID="txtactualdate" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpDate" aria-describedby="inputSuccess2Status3"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                    <span id="spantxtactualdate" class="sr-only">(success)</span>
                                                    <span class="error invalid-feedback"><%= AppModels.ErrorMessage.invaliddate %></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Amount</label>
                                                <asp:TextBox ID="txtfundliquidationamount" runat="server" CssClass="form-control rounded-0 text-right">
                                                </asp:TextBox>
                                                <span class="error invalid-feedback">
                                                    <asp:Literal ID="errortxtfundliquidationamount" runat="server"></asp:Literal></span>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Receipt</label><span class="form-validation"><asp:Literal ID="errorimagerequired" runat="server" Text="*Receipt image is required!" Visible="false"></asp:Literal></span>
                                                <div style="text-align: center">
                                                    <asp:Button ID="btnUpload" runat="server" CssClass="btnsaveimage" CausesValidation="false" OnClick="btnUpload_Click" Style="display: none;" />
                                                    <asp:Literal ID="literalimagefilename" runat="server" Text="" Visible="false"></asp:Literal>
                                                    <asp:Image ID="imgpreview" runat="server" Height="60" Width="60" ImageUrl="#" Style="border-width: 0px;" Visible="false" CssClass="imgpreview" data-toggle="modal" data-target=".imagepreviewmodal" />
                                                    <a onclick="fnFireEventClass('click','.fureceipt');"><i class="fa fa-upload"></i></a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 form-group">
                                            <asp:LinkButton ID="btncancelfundliquidation" runat="server" CssClass="btn btn-default btn-sm btn-flat float-right" CausesValidation="false" OnClick="btncancelfundliquidation_Click">Cancel</asp:LinkButton>
                                            <asp:LinkButton ID="btnsavefundliquidation" runat="server" CssClass="btn btn-success btn-sm btn-flat float-right" Style="margin-right: 5px;" CausesValidation="false" OnClick="btnsavefundliquidation_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.card-body -->
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </asp:Panel>

<button type="button" id="btndisbursementmodal" data-toggle="modal" data-target=".disbursementmodal" hidden="hidden"></button>
<div class="modal fade disbursementmodal" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="upaneldisbursement" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label>Payment Mode</label>
                                <asp:DropDownList ID="cmbdisburmentmode" runat="server" CssClass="custom-select rounded-0">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Date</label>

                                <div class="input-group date" data-target-input="nearest">
                                    <asp:TextBox ID="txtdisbursementdate" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0" aria-describedby="inputSuccess2Status3" ReadOnly="true"></asp:TextBox>
                                    <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                    <span id="spantxtdisbursementdate" class="sr-only">(success)</span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>Reference</label>
                                <asp:TextBox ID="txtdisbursementreference" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                            </div>
                            <div class="form-group">
                                <label>Remarks</label>
                                <asp:TextBox ID="txtdisbursementremarks" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control rounded-0"></asp:TextBox>
                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                            </div>
                        </div>
                        <div class="modal-footer text-right">
                            <asp:Button ID="btnsavedisbursement" runat="server" CssClass="btn btn-primary btn-flat" UseSubmitBehavior="false"
                                CausesValidation="false" OnClick="btnsavedisbursement_Click" Text="Save" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnsavedisbursement" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <asp:FileUpload ID="fureceipt" runat="server" onchange="fnFireEventClass('click','.btnsaveimage');" CssClass="fureceipt" accept="image/png, image/jpeg" Style="display: none;" />
    <button type="button" id="btnfundrequestmodal" data-toggle="modal" data-target=".fundrequestmodal" hidden="hidden"></button>
    <div class="modal fade fundrequestmodal" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="upanelfundrequestmodal" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label>Remarks</label>
                                <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control rounded-0"></asp:TextBox>
                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                            </div>
                        </div>
                        <div class="modal-footer text-right">
                            <asp:Button ID="btnSaveFundAction" runat="server" CssClass="btn btn-primary btn-flat" UseSubmitBehavior="false"
                                CausesValidation="false" OnClick="btnSaveFundAction_Click" Text="Save changes" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSaveFundAction" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <button type="button" id="btnroutingmodal" data-toggle="modal" data-target=".routingmodal" hidden="hidden"></button>
    <div class="modal fade routingmodal" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="upanelmodalrouting" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <asp:GridView runat="server" ID="dgvmodalrouting"
                                AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true"
                                EmptyDataText="No record added"
                                GridLines="None"
                                CssClass="table table-bordered table-hover dataTable dtr-inline">
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToDateTime(Eval("traildate")).ToString(AppModels.datetimeformat) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employee" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblname" runat="server" Text='<%# Eval("employee.firstname").ToString() + " " + Eval("employee.lastname").ToString() %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <span><%# Eval("remarks") %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <label class='<%# getRequestStatusBadge((int)Eval("statusid")) %>'><%# AppModels.Status.getStatus(Convert.ToInt32(Eval("statusid"))) %></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer text-right">
                            <asp:Button ID="btncloseroutingmodal" runat="server" CssClass="btn btn-default btn-flat" UseSubmitBehavior="false" data-dismiss="modal" CausesValidation="false" Text="Close" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>