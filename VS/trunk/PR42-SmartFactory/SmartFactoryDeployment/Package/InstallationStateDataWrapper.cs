using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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
    public string OwnerLogin
    {
      get
      {
        return State.OwnerLogin;
      }
      internal set
      {
        State.OwnerLogin = value;
      }
    }
    /// <summary>
    /// Gets or sets the owner email.
    /// </summary>
    /// <value>
    /// The owner email.
    /// </value>
    public string OwnerEmail
    {
      get
      {
        return State.OwnerEmail;
      }
      set
      {
        State.OwnerEmail = value;
      }
    }
    /// <summary>
    /// Gets or sets the site collection URL.
    /// </summary>
    /// <value>
    /// The site collection URL.
    /// </value>
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
    [Description("")]
    public Uri WebApplicationURL
    {
      get
      {
        return new Uri(State.WebApplicationURL);
      }
      set
      {
        this.State.WebApplicationURL = value.ToString(); ;
      }
    }
    /// <summary>
    /// Gets the array of solutions.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public Solution[] Solutions
    {
      get
      {
        return State.Solutions;
      }
    }
    #endregion

    #region private
    internal protected InstallationStateData State { get; private set; }
    internal InstallationStateDataWrapper(InstallationStateData _state)
    {
      State = _state;
    }
    #endregion
  }
}
