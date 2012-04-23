using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.SharePoint.Administration;
using CAS.SmartFactory.Deployment.Properties;
using CAS.SmartFactory.Deployment.Controls;
using System.Security.Principal;

namespace CAS.SmartFactory.Deployment.Package
{
  /// <summary>
  /// Installation state data wrapper
  /// </summary>
  public abstract class InstallationStateDataWrapper
  {
    #region public properties

    /// <summary>
    /// Gets the owner login.
    /// </summary>
    [Category("Site collection")]
    [DisplayName("Site owner login")]
    [Description(@"Smart Factory site collection owner login. A string that contains the user name of the owner of the site collection. For example, Domain\User. " +
      "In Active Directory Domain Services account creation mode, the Pwner Login must contain a value even if the value does not correspond to an actual user name.")]
    public string OwnerLogin
    {
      get
      {
        if (String.IsNullOrEmpty(State.OwnerLogin))
        {
          WindowsIdentity _id = WindowsIdentity.GetCurrent();
          State.OwnerLogin = _id.Name;
        }
        return State.OwnerLogin;
      }
      set
      {
        State.OwnerLogin = value;
      }
    }
    /// <summary>
    /// Gets or sets the name of the owner.
    /// </summary>
    /// <value>
    /// The name of the owner.
    /// </value>
    [Category("Site collection")]
    [DisplayName("Site owner name")]
    [Description(@"A string that contains the display name of the owner of the site.")]
    public string OwnerName
    {
      get
      {
        if (String.IsNullOrEmpty(State.OwnerName))
        {
          WindowsIdentity _id = WindowsIdentity.GetCurrent();
          State.OwnerName = _id.Name;
        }
        return State.OwnerName;
      }
      set
      {
        State.OwnerName = value;
      }
    }
    /// <summary>
    /// Gets or sets the owner email.
    /// </summary>
    /// <value>
    /// The owner email.
    /// </value>
    [Category("Site collection")]
    [DisplayName("Site owner email")]
    [Description(@"Site collection owner email address. A string that contains the e-mail address of the owner of the site collection. " +
      "For example someone@example.com")]
    public string OwnerEmail
    {
      get
      {
        if (String.IsNullOrEmpty(State.OwnerEmail))
          State.OwnerEmail = "<someone>@example.com";
        return State.OwnerEmail;
      }
      set
      {
        string _errorMsg;
        if (ValidEmailAddress(value, out _errorMsg))
        {
          State.OwnerEmail = value;
          return;
        }
        // Set the ErrorProvider error with the text to display. 
        throw new ArgumentException(_errorMsg);
      }
    }
    /// <summary>
    /// Gets or sets the site collection URL.
    /// </summary>
    /// <value>
    /// The site collection URL.
    /// </value>
    [Category("Site collection")]
    [DisplayName("Site collection path")]
    [Description("A String that contains the URL for the site collection, for example, Site_Name or sites/Site_Name." +
      "It may either be server-relative or absolute for typical sites.")]
    public string SiteCollectionURL
    {
      get
      {
        return State.SiteCollectionURL;
      }
      set
      {
        State.SiteCollectionURL = value;
      }
    }
    /// <summary>
    /// Gets a value indicating whether the site collection has been already created.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if site collection has been already created; otherwise, <c>false</c>.
    /// </value>
    [Category("Site collection")]
    [ReadOnly(true)]
    [DisplayName("Site created")]
    [Description("True if the site collection has been already created.")]
    public bool SiteCollectionCreated
    {
      get
      {
        return State.SiteCollectionCreated;
      }
      internal set
      {
        this.SiteCollectionCreated = value;
      }
    }
    /// <summary>
    /// Gets or sets the web application URL.
    /// </summary>
    /// <value>
    /// A String that contains the URL for the site collection, for example, Site_Name or sites/Site_Name. 
    /// It may either be server-relative or absolute for typical sites.
    /// </value>
    [Browsable(true)]
    [Category("Application")]
    [Description("A string that specifies the URL of the Web application. For example 'http://computer.domain:Port'.")]
    [DisplayName("Web application URL")]
    [TypeConverter(typeof(UriTypeConverter))]
    public Uri WebApplicationURL
    {
      get
      {
        if (String.IsNullOrEmpty(State.WebApplicationURL))
        {
          try
          {
            Uri _waurl = new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + SPServer.Local.Address);
            State.WebApplicationURL = _waurl.ToString();
            return _waurl;
          }
          catch (Exception) { }
          Tracing.TraceEvent.TraceWarning(111, "WebApplicationURL", Resources.CannotGetAccessToLocalServer);
          return new Uri(@"http://server.domain:12345/path");
        }
        else
          return new Uri(State.WebApplicationURL);
      }
      set
      {
        this.State.WebApplicationURL = value.ToString();
      }
    }
    /// <summary>
    /// Gets or sets the site template.
    /// </summary>
    /// <value>
    /// The site template.
    /// </value>
    [Browsable(true)]
    [Category("Site collection")]
    [Description("A string that specifies the site definition or site template for the site object. Specify null to create a site without applying a template to it.")]
    [DisplayName("Site Template")]
    public string SiteTemplate
    {
      get
      {
        return State.SiteTemplate;
      }
      set
      {
        this.State.SiteTemplate = value;
      }
    }
    /// <summary>
    /// Gets or sets the LCID.
    /// </summary>
    /// <value>
    /// The LCID - an unsigned 32-bit integer that specifies the LCID for the site object.
    /// </value>
    [Browsable(true)]
    [Category("Site collection")]
    [Description("An unsigned 32-bit integer that specifies the LCID for the site object.")]
    [DisplayName("LCID")]
    public uint LCID
    {
      get
      {
        return State.LCID;
      }
      set
      {
        State.LCID = value;
      }
    }
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    [Browsable(true)]
    [Category("Site collection")]
    [Description("A string that contains the title of the site object.")]
    [DisplayName("Title")]
    public string Title
    {
      get
      {
        return State.Title;
      }
      set
      {
        State.Title = value;
      }
    }
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    [Browsable(true)]
    [Category("Site collection")]
    [Description("A string that contains the description for the site object.")]
    [DisplayName("Description")]
    public string Description
    {
      get
      {
        return State.Description;
      }
      set
      {
        State.Description = value;
      }
    }
    /// <summary>
    /// Gets the array of solutions.
    /// </summary>
    [TypeConverter(typeof(CollectionConverter))]
    [Category("Content")]
    [Description("Solutions included in this package. ")]
    public List<Solution> Solutions
    {
      get
      {
        return State.Solutions.ToList();
      }
    }
    #endregion

    #region Validating
    private static bool ValidEmailAddress(string _emailAddress, out string _errorMessage)
    {
      // Confirm that the e-mail address string is not empty.
      if (_emailAddress.Length == 0)
      {
        _errorMessage = "e-mail address is required.";
        return false;
      }
      // Confirm that there is an "@" and a "." in the e-mail address, and in the correct order.
      if (_emailAddress.IndexOf("@") > -1)
      {
        if (_emailAddress.IndexOf(".", _emailAddress.IndexOf("@")) > _emailAddress.IndexOf("@"))
        {
          _errorMessage = "";
          return true;
        }
      }
      _errorMessage = "e-mail address must be valid e-mail address format.\n" +
         "For example 'someone@example.com' ";
      return false;
    }
    #endregion

    #region private
    /// <summary>
    /// Gets the state.
    /// </summary>
    internal protected InstallationStateData State { get; private set; }
    internal InstallationStateDataWrapper(InstallationStateData _state)
    {
      State = _state;
    }
    #endregion
  }
}
