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
            <div class="col-sm-3 form-group">
                <br />
                <%-- //Datetime--%>
                <div class="row form-control ">
                    <asp:Label ID="lblDateTime" runat="server" Style="color: orange; font-weight:bold;display: flex; justify-content: center; align-items: center"></asp:Label>
                </div>
                <br />
                <%--User deails--%>
                <div class="row form-control embed-responsive text-center" style="height: 80px; display: flex; justify-content: center; align-items: center">
                    <asp:Label ID="lblDetail" runat="server" Style="color: brown; font-weight:bold;"></asp:Label>
                </div>&nbsp;
                <%--All Users list--%>
                <div class="row form-control embed-responsive text-center" style="display: flex; justify-content: center; align-items: center" id="UsersPanelList" runat="server">
                    <asp:Panel runat="server" ScrollBars="Vertical" Height="250px" CssClass="form-control">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <table class="table table-condensed table-hover table-responsive" style="margin: 0px; padding: 0px;">
                                    <tr class="row">
                                        <th class="col-10">
                                            <asp:Label runat="server" ID="usersList" Text='<%# Eval("Email") %>'></asp:Label>
                                        </th>
                                        <td class="col-2">
                                            <asp:Button runat="server" ID="deleteUser" CommandArgument='<%# Eval("Role") %>' Text="Delete" CssClass="btn btn-danger" OnClick="DeleteUser_Click"  OnClientClick="return confirm('Are you sure you want to delete this user?');" style="height:25px;font-size:10px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </div>
                <%--Logout Action button--%>
                <div class="row">
                    <asp:Button runat="server" ID="btnLogout" CssClass="btn btn-danger" Text="Logout" style="margin-block: 5px; width:100px" OnClick="btnLogout_Click" />
                </div>
            </div>
            <div class="col-sm-6 ">
                <div class="row">
                    <div class="row" style="display: flex; justify-content: center; align-items: center">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-10" style="display: flex; justify-content: center; align-items: center">
                                    <%--File Name--%>
                                    <asp:TextBox ID="lblCreate" runat="server" CssClass="form-control" placeholder="Enter File Name..." Style="margin-top: 20px;"></asp:TextBox>
                                    <%--Save Action Button--%>
                                    <asp:Button ID="btSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btSave_Click" Style="margin: 5px; margin-top: 25px" />
                                    <%--Close button--%>
                                    <asp:LinkButton ID="btnClose" runat="server" Text="Close" CssClass="btn btn-warning" PostBackUrl="~/Notepad.aspx" Style="margin-top: 20px;" />
                                </div>
                                <div class="col-sm-2"></div>
                            </div>
                        </div>
                    </div>
                    <%--Text Content entered by user--%>
                    <div class="form-group">
                        <asp:TextBox runat="server" ID="txtNote" placeholder="Enter your thoughts here..." Rows="15" Columns="100" CssClass="form-control" TextMode="MultiLine" BorderStyle="Solid" Style="margin-top: 10px"></asp:TextBox>
                    </div>
                    <%--Empty alert if path not found of a file--%>
                    <div class="form-group">
                        <asp:Label runat="server" ID="emptyalert" CssClass="form-control text-center" ForeColor="OrangeRed"></asp:Label>
                    </div>
                    <br />
                </div>
            </div>
           
     <div class="col-sm-3 form-group" style="margin-top:35px">
         <h3 style=" font-family: 'Calisto MT'; color: goldenrod">Saved Files...</h3>
         <div class="row" >
             <asp:Label runat="server" ID="folderPath"></asp:Label>
             <%--Saved files list--%>
             <asp:Panel runat="server" ScrollBars="Vertical" Height="370px" CssClass="form-control">
                 <asp:Repeater ID="repeatFiles" runat="server">
                     <ItemTemplate>
                         <table class="table table-condensed table-hover table-responsive" style="font-size:15px;margin: 0px; padding: 0px;">
                             <tr class="row">
                                 <th class="col-6">
                                     <asp:LinkButton ID="btnOpen" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="btnOpen_Click" style="font-size:12px">
                                         <%# Eval("Name") %>
                                     </asp:LinkButton>
                                 </th>
                                 <td  class="col-3">
                                     <asp:Button runat="server" ID="btnDelete" CommandArgument='<%# Eval("Id") %>' Text="Delete" CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this file.');" OnClick="btnDelete_Click" style="height:25px;font-size:10px;"/>
                                 </td>
                                 <td class="col-3">
                                     <asp:Button runat="server" ID="btnExport" CommandArgument='<%# Eval("Id") %>' Text="Get PDF" CssClass="btn btn-dark"  OnClick="Export_Click"  style="height:25px;font-size:10px;"/>
                                 </td>
                             </tr>
                         </table>
                     </ItemTemplate>
                 </asp:Repeater>
             </asp:Panel>
         </div>
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
