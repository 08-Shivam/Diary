<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notepad.aspx.cs" Inherits="Diary.Notepad" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="row">
                <p class="h2 text-center" style="border-bottom: groove; padding: 5px; color: goldenrod">
                    <span style="color: burlywood; font-family: 'Calisto MT'">Digital</span> Diary
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 col-md-3 col-lg-3 form-group">
                <br />
                <div class="row form-control ">
                    <asp:Label ID="lblDateTime" runat="server" Style="color: orange; display: flex; justify-content: center; align-items: center"></asp:Label>
                </div>
                <br />
                <div class="row form-control embed-responsive text-center" style="height: 50px; display: flex; justify-content: center; align-items: center">
                    <asp:Label ID="lblDetail" runat="server" Style="color: brown"></asp:Label>
                </div>
                <div class="row">
                    <asp:Button runat="server" ID="btnLogout" CssClass="btn btn-danger" Text="Logout" Style="margin-block: 5px" OnClick="btnLogout_Click" />
                </div>
            </div>
            <div class="col-sm-6 col-md-6 col-lg-6">
                <div class="row">
                    <div class="row" style="display: flex; justify-content: center; align-items: center">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-10" style="display: flex; justify-content: center; align-items: center">
                                    <asp:TextBox ID="lblCreate" runat="server" CssClass="form-control" placeholder="Enter File Name..." Style="margin-top: 20px;"></asp:TextBox>
                                    <asp:Button ID="btSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btSave_Click" Style="margin: 5px; margin-top: 25px" />
                                    <asp:LinkButton ID="btnClose" runat="server" Text="Close" CssClass="btn btn-warning" OnClick="btnClose_Click" PostBackUrl="~/Notepad.aspx" Style="margin-top: 20px;" />
                                </div>
                                <div class="col-sm-2"></div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:TextBox runat="server" ID="txtNote" placeholder="Enter your thoughts here..." Rows="15" Columns="80" CssClass="form-control" TextMode="MultiLine" BorderStyle="Solid" Style="margin-top: 10px"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="emptyalert" CssClass="form-control text-center" ForeColor="OrangeRed"></asp:Label>
                    </div>
                    <br />
                </div>
            </div>
            &nbsp;
     <div class="col-sm-3 col-md-3 col-lg-3 form-group">
         <h3 style="margin-left: 10px; font-family: 'Calisto MT'; color: goldenrod">Saved Files...</h3>
         <asp:Label runat="server" ID="folderPath"></asp:Label>
         <asp:Panel runat="server" ScrollBars="Vertical" Height="310px" CssClass="form-control">
             <asp:Repeater ID="repeatFiles" runat="server">
                 <ItemTemplate>
                     <table class="table table-condensed table-hover" style="margin: 0px; padding: 0px;">
                         <tr>
                             <td>
                                 <asp:LinkButton ID="btnOpen" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="btnOpen_Click" CssClass="btn btn-link p-0">
                                         <%# Eval("Name") %>
                                 </asp:LinkButton>
                             </td>
                             <td style="text-align: center;">
                                 <asp:Button runat="server" ID="btnDelete" CommandArgument='<%# Eval("Id") %>' Text="Delete" CssClass="btn btn-danger btn-xs " Style="margin-top: 5px" OnClick="btnDelete_Click" />
                             </td>
                         </tr>
                     </table>
                 </ItemTemplate>
             </asp:Repeater>
         </asp:Panel>
     </div>
        </div>
    </div>
    <script type="text/javascript">
        function updateDateTime() {
            var now = new Date();
            var formattedDate = now.toLocaleString();
            document.getElementById('<%= lblDateTime.ClientID %>').innerText = formattedDate;
        }
        setInterval(updateDateTime, 1000);
    </script>
</asp:Content>
