<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CallReportDetailInfo.aspx.cs" Inherits="SimpleFFO.views.CallReportDetailInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

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
                        <h3 class="card-title">Doctor Info</h3>
                    </div>

                    <div class="card-body">

                        <div class="row">

                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label for="txtbox_docCode">Doctor Code</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_docode" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label for="txtbox_fullname">Doctor Name</label>
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
                                    <label for="txtbox_specialization">Specialization</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_specialization" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label for="txtbox_birthday">Birthday</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_birthday" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label for="txtbox_contact">Contact Number</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-phone"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_contact" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label for="txtbox_License">PRC License</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_license" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label for="txtbox_emailaddress">email address</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_emailAddress" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>


                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label for="txtbox_Institution">Institution</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-building"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_institution" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
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
                                            <label for="txtbox_planned">Planned?</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtbox_planned" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label for="txtbox_makeup">Makeup?</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtbox_makeup" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
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


                                <div class="form-group">
                                    <label for="txtbox_signature">Signature</label>
                                    <div class="input-group">
                                        <asp:Image runat="server" ID="imgSRC" ImageUrl="#" Style="max-width: 100%" data-toggle="modal" data-target=".imagepreviewmodal" onclick="fnshowimgpreview(this,'#imgmainpreview');"/>
                                    </div>
                                </div>



                            </div>

                        </div>
                    </div>

                    <div class="col-md-6">

                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Call Notes</h3>
                            </div>

                            <div class="card-body">

                                <%--  <asp:BulletedList runat="server" ID="bltImagesURL" DisplayMode="HyperLink" Target="_blank">
                                                    </asp:BulletedList>--%>

                                <asp:BulletedList runat="server" ID="lstnotes" Width="100%" Height="290">
                                </asp:BulletedList>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Location</h3>
                    </div>

                    <div class="card-body">

                        <iframe id="urlframe" runat="server" height="500" frameborder="0" style="border: 0; width: 100%"></iframe>

                    </div>

                </div>




            </div>


        </section>


    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">

    <%-- <style type="text/css">
        html,body,#canvasMap{
            height: 100%;
            margin: 0px;
            padding: 0px;
        }
    </style>

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyD5Rdf2Ord2pRC67NxeCQtN8dKnTaOXUbM"></script>
    <script type="text/javascript">  
        var map;  
        function LoadGoogleMAP() {  
            var SetmapOptions = {  
                zoom: 10,  
                center: new google.maps.LatLng(-34.397, 150.644)  
            };  
            map = new google.maps.Map(document.getElementById('canvasMap'),  
      SetmapOptions);  
        }  
  
        google.maps.event.addDomListener(window, 'load', LoadGoogleMAP);  
  
    </script>  --%>
</asp:Content>
