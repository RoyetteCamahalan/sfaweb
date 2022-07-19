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
                                <asp:Literal ID="lblModalTitle" runat="server">New Shop</asp:Literal></h4>
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span></button>
                        </div>
                       <div class="modal-body">
                        <div class="row">

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="txtvendorname">Vendor name <span class="required">*</span></label>
                                    <asp:TextBox ID="txtvendorname" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    <span class="error invalid-feedback">
                                        <asp:Literal ID="lblerrorvendorname" runat="server"></asp:Literal></span>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <div class="form-group">
                                        <label for="txtaddress">Address <span class="required">*</span></label>
                                        <asp:TextBox ID="txtaddress" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="txttelephone">Telephone <span class="required">*</span></label>
                                    <asp:TextBox ID="txttelephone" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="txtvatno">Vat number <span class="required">*</span></label>
                                    <asp:TextBox ID="txtvatno" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                    <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                </div>
                            </div>


                            <div class="col-sm-4">
                                <div class="form-group">
                                    <div class="form-group">
                                        <label for="txtvar">is vat? <span class="required">*</span></label>
                                        <asp:DropDownList ID="cmbisvat" runat="server" CssClass="custom-select rounded-0" TabIndex="-1"
                                             AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="0" Text="--Select--"></asp:ListItem>
                                            <asp:ListItem Selected="False" Value="1" Text="VAT"></asp:ListItem>
                                            <asp:ListItem Selected="False" Value="2" Text="Non-VAT"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                    </div>
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
                <asp:AsyncPostBackTrigger ControlID="cmbempbranches" />
            </Triggers>
        </asp:UpdatePanel>
    </div>