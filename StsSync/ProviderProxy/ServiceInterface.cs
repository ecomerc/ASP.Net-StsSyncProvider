﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1434
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.1432.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
[System.Web.Services.WebServiceBindingAttribute(Name="ListsSoap", Namespace="http://schemas.microsoft.com/sharepoint/soap/")]
public interface IListsSoap {
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetList", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetList(string listName);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListAndView", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetListAndView(string listName, string viewName);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/DeleteList", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    void DeleteList(string listName);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/AddList", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode AddList(string listName, string description, int templateID);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/AddListFromFeature", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode AddListFromFeature(string listName, string description, System.Guid featureID, int templateID);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateList", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode UpdateList(string listName, System.Xml.XmlNode listProperties, System.Xml.XmlNode newFields, System.Xml.XmlNode updateFields, System.Xml.XmlNode deleteFields, string listVersion);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListCollection", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetListCollection();
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListItems", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetListItems(string listName, string viewName, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, string rowLimit, System.Xml.XmlNode queryOptions, string webID);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListItemChanges", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetListItemChanges(string listName, System.Xml.XmlNode viewFields, string since, System.Xml.XmlNode contains);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListItemChangesSinceToken", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetListItemChangesSinceToken(string listName, string viewName, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, string rowLimit, System.Xml.XmlNode queryOptions, string changeToken, System.Xml.XmlNode contains);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateListItems", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode UpdateListItems(string listName, System.Xml.XmlNode updates);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/AddDiscussionBoardItem", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode AddDiscussionBoardItem(string listName, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] message);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetVersionCollection", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetVersionCollection(string strlistID, string strlistItemID, string strFieldName);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/AddAttachment", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    string AddAttachment(string listName, string listItemID, string fileName, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] attachment);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetAttachmentCollection", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetAttachmentCollection(string listName, string listItemID);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/DeleteAttachment", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    void DeleteAttachment(string listName, string listItemID, string url);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/CheckOutFile", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    bool CheckOutFile(string pageUrl, string checkoutToLocal, string lastmodified);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UndoCheckOut", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    bool UndoCheckOut(string pageUrl);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/CheckInFile", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    bool CheckInFile(string pageUrl, string comment, string CheckinType);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListContentTypes", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetListContentTypes(string listName, string contentTypeId);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/GetListContentType", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode GetListContentType(string listName, string contentTypeId);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/CreateContentType", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    string CreateContentType(string listName, string displayName, string parentType, System.Xml.XmlNode fields, System.Xml.XmlNode contentTypeProperties, string addToView);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateContentType", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode UpdateContentType(string listName, string contentTypeId, System.Xml.XmlNode contentTypeProperties, System.Xml.XmlNode newFields, System.Xml.XmlNode updateFields, System.Xml.XmlNode deleteFields, string addToView);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/DeleteContentType", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode DeleteContentType(string listName, string contentTypeId);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateContentTypeXmlDocument", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode UpdateContentTypeXmlDocument(string listName, string contentTypeId, System.Xml.XmlNode newDocument);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/UpdateContentTypesXmlDocument", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode UpdateContentTypesXmlDocument(string listName, System.Xml.XmlNode newDocument);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/DeleteContentTypeXmlDocument", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode DeleteContentTypeXmlDocument(string listName, string contentTypeId, string documentUri);
    
    /// <remarks/>
    [System.Web.Services.WebMethodAttribute()]
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/sharepoint/soap/ApplyContentTypeToList", RequestNamespace="http://schemas.microsoft.com/sharepoint/soap/", ResponseNamespace="http://schemas.microsoft.com/sharepoint/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    System.Xml.XmlNode ApplyContentTypeToList(string webUrl, string contentTypeId, string listName);
}
