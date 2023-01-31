using System.ComponentModel;

namespace CIR.Common.Enums
{
    public enum RolePriviledgesEnums
    {
        [Description("Users - View List")]
        User_ViewList = 1,

        [Description("Users - Create")]
        User_Create,

        [Description("Users - Edit")]
        User_Edit,

        [Description("Users - Delete")]
        User_Delete,

        [Description("Users - View Details")]
        Users_ViewDetails,

        [Description("Users - Permissions")]
        Users_Permissions,

        [Description("Users - Dashboard Tile")]
        Users_DashboardTile,

        [Description("Advanced Prices - Create")]
        AdvancedPrices_Create,

        [Description("Advanced Prices - Delete")]
        AdvancedPrices_Delete,

        [Description("Advanced Prices - Edit")]
        AdvancedPrices_Edit,

        [Description("Advanced Prices - Index")]
        AdvancedPrices_Index,

        [Description("Advanced Stock - Create")]
        AdvancedStock_Create,

        [Description("Advanced Stock - Delete")]
        AdvancedStock_Delete,

        [Description("Advanced Stock - EditAdvanced Stock - Index")]
        AdvancedStock_EditAdvancedStock_Index,

        [Description("Analytics - Dashboard Tile")]
        Analytics_DashboardTile,

        [Description("Analytics - View List")]
        Analytics_ViewList,

        [Description("Articles - Dashboard Tile")]
        Articles_DashboardTile,

        [Description("Audit - Index")]
        Audit_Index,

        [Description("Audit trails - Dashboard Tile")]
        Audittrails_DashboardTile,

        [Description("BBC News - Dashboard Tile")]
        BBCNews_DashboardTile,

        [Description("Career - Edit")]
        Career_Edit,

        [Description("Career - Index")]
        Career_Index,

        [Description("CMS Configuration")]
        CMS_Configuration,

        [Description("Code - Create")]
        Code_Create,

        [Description("Code - Delete")]
        Code_Delete,

        [Description("Code - Edit")]
        Code_Edit,

        [Description("Code - View List")]
        Code_ViewList,

        [Description("Content - Cloner")]
        Content_Cloner,

        [Description("Content - Disable")]
        Content_Disable,

        [Description("Content - Editor")]
        Content_Editor,

        [Description("Content - Index")]
        Content_Index,

        [Description("Content - Preview")]
        Content_Preview,

        [Description("Content - Publisher")]
        Content_Publisher,

        [Description("Content - Restore")]
        Content_Restore,

        [Description("Corporate - Add Office Member")]
        Corporate_AddOfficeMember,

        [Description("Corporate - Add Staff Member")]
        Corporate_AddStaffMember,

        [Description("Corporate - Delete Office Member")]
        Corporate_DeleteOfficeMember,

        [Description("Corporate - Delete Staff Member")]
        Corporate_DeleteStaffMember,

        [Description("Corporate - Edit Office Member")]
        Corporate_EditOfficeMember,

        [Description("Corporate - Edit Staff Member")]
        Corporate_EditStaffMember,

        [Description("Corporate - View Office List")]
        Corporate_ViewOfficeList,

        [Description("Corporate - View Staff List")]
        Corporate_ViewStaffList,

        [Description("Dark - Index")]
        Dark_Index,

        [Description("Deleted pages - Enable page")]
        Deletedpages_Enablepage,

        [Description("Deleted pages - View List")]
        Deletedpages_ViewList,

        [Description("Discount - Create")]
        Discount_Create,

        [Description("Discount - Delete")]
        Discount_Delete,

        [Description("Discount - Edit")]
        Discount_Edit,

        [Description("Discount - Index")]
        Discount_Index,

        [Description("Easypay - Index")]
        Easypay_Index,

        [Description("eCommerce - Dashboard Tile")]
        eCommerce_DashboardTile,

        [Description("Ecommerce - Index")]
        Ecommerce_Index,

        [Description("Email - Campaign Editor")]
        Email_CampaignEditor,

        [Description("Email - Group/Department Editor")]
        Email_GroupDepartmentEditor,

        [Description("Email - Subscriber Editor")]
        Email_SubscriberEditor,

        [Description("Email - Template Editor")]
        Email_TemplateEditor,

        [Description("Events - Index")]
        Events_Index,

        [Description("iChannels - Index")]
        iChannels_Index,

        [Description("Language - Change")]
        Language_Change,

        [Description("Languages - View List")]
        Languages_ViewList,

        [Description("Lookups - Clone")]
        Lookups_Clone,

        [Description("Lookups - Create")]
        Lookups_Create,

        [Description("Lookups - Edit")]
        Lookups_Edit,

        [Description("Lookups - View List")]
        Lookups_ViewList,

        [Description("Mail - Index")]
        Mail_Index,

        [Description("Mail Centre - Dashboard Tile")]
        MailCentre_DashboardTile,

        [Description("Manual Order - Create")]
        ManualOrder_Create,

        [Description("Manual Order - Index")]
        ManualOrder_Index,

        [Description("Media - Dashboard Tile")]
        Media_DashboardTile,

        [Description("Media - Folder Options")]
        Media_FolderOptions,

        [Description("Media - Index")]
        Media_Index,

        [Description("Media - Quick Upload")]
        Media_QuickUpload,

        [Description("Messages - Create")]
        Messages_Create,

        [Description("Messages - Delete")]
        Messages_Delete,

        [Description("Messages - Edit")]
        Messages_Edit,

        [Description("Messages - View List")]
        Messages_ViewList,

        [Description("Meta Tags - Create")]
        MetaTags_Create,

        [Description("Meta Tags - Delete")]
        MetaTags_Delete,

        [Description("Meta Tags - Edit")]
        MetaTags_Edit,

        [Description("Meta Tags - View List")]
        MetaTags_ViewList,

        [Description("NewsAlert pages - NewsAlert page")]
        NewsAlertpages_NewsAlertpage,

        [Description("Notification - Dashboard Tile")]
        Notification_DashboardTile,

        [Description("Notifications - Index")]
        Notifications_Index,

        [Description("Order - Edit")]
        Order_Edit,

        [Description("Order - Index")]
        Order_Index,

        [Description("Payment gateway - Index")]
        Paymentgateway_Index,

        [Description("Permissions - Asset")]
        Permissions_Asset,

        [Description("Permissions - Page")]
        Permissions_Page,

        [Description("Portal Admin")]
        Portal_Admin,

        [Description("Portal Orders")]
        PortalOrders,

        [Description("Portal Orders Admin Cancel")]
        PortalOrdersAdmin_Cancel,

        [Description("Portal Orders Admin Edit")]
        PortalOrdersAdmin_Edit,

        [Description("Portal Orders Admin Restart")]
        PortalOrdersAdmin_Restart,

        [Description("Portal Orders Cancel")]
        PortalOrders_Cancel,

        [Description("Portal Orders Complete")]
        PortalOrders_Complete,

        [Description("Portal Orders Create")]
        PortalOrders_Create,

        [Description("Portal Orders Edit")]
        PortalOrders_Edit,

        [Description("Portal Orders Edit Reference Number")]
        PortalOrders_EditReferenceNumber,

        [Description("Portal Orders Label")]
        PortalOrders_Label,

        [Description("Portal Orders Restart")]
        PortalOrders_Restart,

        [Description("Portal Orders Track")]
        PortalOrders_Track,

        [Description("Portal Orders Update Status")]
        PortalOrders_UpdateStatus,

        [Description("Product - Create")]
        Product_Create,

        [Description("Product - Edit")]
        Product_Edit,

        [Description("Product - Index")]
        Product_Index,

        [Description("Product Attribute - Create")]
        ProductAttribute_Create,

        [Description("Product Attribute - Edit")]
        ProductAttribute_Edit,

        [Description("Product Attribute - Index")]
        ProductAttribute_Index,

        [Description("Product attributes - Create")]
        Productattributes_Create,

        [Description("Product attributes - Edit")]
        Productattributes_Edit,

        [Description("Product attributes - Index")]
        Productattributes_Index,

        [Description("Product bundle - Create")]
        Productbundle_Create,

        [Description("Product bundle - Delete")]
        Productbundle_Delete,

        [Description("Product bundle - Edit")]
        Productbundle_Edit,

        [Description("Product bundle - Index")]
        Productbundle_Index,

        [Description("Redirects - Create")]
        Redirects_Create,

        [Description("Redirects - Delete")]
        Redirects_Delete,

        [Description("Redirects - Edit")]
        Redirects_Edit,

        [Description("Redirects - View List")]
        Redirects_ViewList,

        [Description("Reports - View List")]
        Reports_ViewList,

        [Description("Roles - Create")]
        Roles_Create,

        [Description("Roles - Delete")]
        Roles_Delete,

        [Description("Roles - Edit")]
        Roles_Edit,

        [Description("Roles - View Details")]
        Roles_ViewDetails,

        [Description("Roles - View List")]
        Roles_ViewList,

        [Description("Section/Area - Create")]
        SectionArea_Create,

        [Description("Section/Area - Delete")]
        SectionArea_Delete,

        [Description("Section/Area - Edit")]
        SectionArea_Edit,

        [Description("Section/Area - View List")]
        SectionArea_ViewList,

        [Description("Shipping Manager - Index")]
        ShippingManager_Index,

        [Description("Social - Index")]
        Social_Index,

        [Description("Stock Levels - Index")]
        StockLevels_Index,

        [Description("Style Guide - Index")]
        StyleGuide_Index,

        [Description("Tax - Create")]
        Tax_Create,

        [Description("Tax - Edit")]
        Tax_Edit,

        [Description("Tax - Index")]
        Tax_Index,

        [Description("Twitter - Dashboard Tile")]
        Twitter_DashboardTile,

        [Description("Warehouse - Create")]
        Warehouse_Create,

        [Description("Warehouse - Edit")]
        Warehouse_Edit,

        [Description("Warehouse - Index")]
        Warehouse_Index,

        [Description("Weather - Dashboard Tile")]
        Weather_DashboardTile
    }
}
