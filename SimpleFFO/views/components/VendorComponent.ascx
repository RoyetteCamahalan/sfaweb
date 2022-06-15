<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorComponent.ascx.cs" Inherits="SimpleFFO.views.components.VendorComponent" %>
<%@ Import Namespace="SimpleFFO.Model" %>
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
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-check float-right">
                                        <asp:CheckBox ID="chkIsactive" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label" for="chkisactive">Is Active?</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblSupplierName" runat="server" AssociatedControlID="txtSupplierName">Vendor Name 
                                                <span class="required">*</span></asp:Label>
                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                                <span class="error invalid-feedback">
                                    <asp:Literal ID="errtxtSupplierName" runat="server"></asp:Literal></span>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbladdress" runat="server" AssociatedControlID="txtaddress">Address
                                                <span class="required">*</span></asp:Label>
                                <asp:TextBox ID="txtaddress" runat="server" CssClass="form-control rounded-0" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="cmbwarehousebranches">Branch <span class="required">*</span></label>
                                <asp:DropDownList ID="cmbsupplierbranches" runat="server" CssClass="custom-select rounded-0" TabIndex="-1">
                                </asp:DropDownList>
                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                            </div>
                                <div class="form-check float-right">
                                    <asp:CheckBox ID="chkvat" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="chkvat">Is Vat?</label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtaddress">T.I.N</asp:Label>
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control rounded-0" required="required" data-inputmask='"mask": "999-999-999-999"' data-mask></asp:TextBox>
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
                <asp:AsyncPostBackTrigger ControlID="cmbempbranches" />
            </Triggers>
        </asp:UpdatePanel>
    </div>