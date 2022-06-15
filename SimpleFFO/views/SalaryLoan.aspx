<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalaryLoan.aspx.cs" Inherits="SimpleFFO.views.SalaryLoan" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagName="StatusTrailComponent" Src="components/StatusTrailComponent.ascx" TagPrefix="uccom"%>
<%@ Register TagName="FundingComponent" Src="components/FundingComponent.ascx" TagPrefix="ucfunding"%>

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
                        <h1>Salary Loan</h1>
                    </div>
                </div>
            </div>
        </section>

        <section class="content">
            <div class="container-fluid">

                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Loan Detail</h3>
                    </div>
                    <div class="card-body">

                        <div class="row">

                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label>Employee Name</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_fullname" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label>Loan Amount *</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-money-bill-alt"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_loanamount" runat="server" CssClass="form-control rounded-0" onkeypress="return isNumberKey(event);" OnTextChanged="TextboxTextChanged" MaxLength="9" AutoPostBack="true" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>



                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>

                                <div class="row">

                                    <div class="col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Loan Amount in Words</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-money-bill-alt"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtbox_loadamountinwords" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label>Payable in Months</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-calendar"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtboxpayableinMonth" Text="6" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-3 col-xs-12">
                                        <div class="form-group">
                                            <label>Deduction per Pay Day</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-money-bill-alt"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtboxdeductionperpayday" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtbox_loanamount" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>





                        <div class="row">

                            <div class="form-group col-sm-6 col-xs-12">
                                <label for="txt_CompanyNotes">Purpose of the Loan *</label>
                                <asp:TextBox ID="txtCompanyNotes" runat="server" CssClass="form-control rounded-0" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>

                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label>Date Hired</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_datehired" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label>Date Applied</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_dateapplied" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>
                </div>

            </div>
            <uccom:StatusTrailComponent ID="ctlStatusTrail" runat="server"></uccom:StatusTrailComponent>
            <ucfunding:FundingComponent ID="ctlFunding" runat="server"></ucfunding:FundingComponent>
        </section>
        <div class="row">
            <div class="col-lg-12 form-group">
                <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-default btn-flat float-right" Style="margin-right: 5px;" Text="Cancel" OnClick="Cancel" />
                <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success btn-flat float-right" Style="margin-right: 5px;" Text="Submit" OnClick="Submit" />
            </div>
        </div>


    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <script type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;
            else {
                var len = document.getElementById("MainContent_txtbox_loanamount").value.length;
                var index = document.getElementById("MainContent_txtbox_loanamount").value.indexOf('.');

                if (index > 0 && charCode == 46) {
                    return false;
                }
                if (index > 0) {
                    var CharAfterdot = (len + 1) - index;
                    if (CharAfterdot > 3) {
                        return false;
                    }
                }

            }
            return true;
        }

    </script>
</asp:Content>
