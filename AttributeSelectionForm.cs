using System;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Diagnostics;
using OSIsoft.AF.UI;

namespace OSIsoft.AF.Asset.DataReference
{
    public partial class AttributeSelectionForm : Form
    {
        private bool _allowElementWithDefaultAttributeSelect;
        private AFDatabase _database;
        private AFElement _rootElement;
        private AFElementTemplate _rootElementTemplate;
        private AFElementTemplates _rootElementTemplates;

        public bool AllowElementWithDefaultAttributeSet
        {
            get { return _allowElementWithDefaultAttributeSelect; }
            set
            {
                _allowElementWithDefaultAttributeSelect = value;
                //this.afTreeView1.AfterSelect();
            }
        }

        public AFElement RootElement
        {
            get { return _rootElement; }
            set
            {
                _rootElement = value;
                try
                {
                    afTreeView1.SetAFRoot(_rootElement, null, null);
                }
                catch (Exception ex)
                {
                    AFTrace.TraceEvent(ex);
                    DrawingHelper.DisplayErrorMessage(this, null, null, ex);
                }
            }
        }

        public AFAttribute SelectedAttribute
        {
            get
            {
                if (afTreeView1.AFSelection as AFAttribute == null)
                {
                    return null;
                }
                else
                {
                    return afTreeView1.AFSelection as AFAttribute;
                }
            }
            set
            {
                try
                {
                    afTreeView1.Select();
                }
                catch (Exception ex)
                {
                    AFTrace.TraceEvent(ex);
                    DrawingHelper.DisplayErrorMessage(this, null, null, ex);
                }
            }
        }

        public AFAttributeTemplate SelectedAttributeTemplate
        {
            get
            {
                if (afTreeView1.AFSelection as AFAttributeTemplate == null)
                {
                    return null;
                }
                else
                {
                    return afTreeView1.AFSelection as AFAttributeTemplate;
                }
            }
            set
            {
                try
                {
                    afTreeView1.AFSelect(value, null, null);
                }
                catch (Exception ex)
                {
                    AFTrace.TraceEvent(ex);
                    DrawingHelper.DisplayErrorMessage(this, null, null, ex);
                }
            }
        }

        public AFElement SelectedElement
        {
            get
            {
                if (afTreeView1.AFSelection as AFElement == null)
                {
                    return null;
                }
                else
                {
                    return afTreeView1.AFSelection as AFElement;
                }
            }
            set
            {
                afTreeView1.AFSelect(value, null, null);
            }
        }

        public AFElementTemplate SelectedElementTemplate
        {
            get
            {
                if (afTreeView1.AFSelection as AFElementTemplate == null)
                {
                    return null;
                }
                else
                {
                    return afTreeView1.AFSelection as AFElementTemplate;
                }
            }
            set
            {
                afTreeView1.AFSelect(value, null, null);
            }
        }

        public string SelectedPath
        {
            get { return afTreeView1.AFSelectedPath; }
        }

        public string RelativePath
        {
            get
            {
                string path = String.Empty;
                if (SelectedAttribute == null)
                {
                    if (SelectedElement == null)
                    {
                        if (SelectedAttributeTemplate != null)
                        {
                            path = SelectedAttributeTemplate.GetPath(SelectedAttributeTemplate.ElementTemplate);
                        }
                    }
                    else
                    {
                        path = SelectedElement.GetPath(_rootElement);
                    }
                }
                else
                {
                    path = SelectedAttribute.GetPath(_rootElement);
                }
                return path;
            }
        }

        public AttributeSelectionForm()
        {
            InitializeComponent();
        }

        public AttributeSelectionForm(AFDatabase database)
            : this()
        {
            if (database != null)
            {
                _database = database;
                SynchUI();
                return;
            }
            else
            {
                throw new ArgumentException("You must specify either an attribute or database.");
            }
        }

        public AttributeSelectionForm(AFElement element)
            : this()
        {
            if (element != null)
            {
                _rootElement = element;
                SynchUI();
                return;
            }
            else
            {
                throw new ArgumentException("You must specify either an attribute or database.");
            }
        }

        public AttributeSelectionForm(AFElementTemplates elementTemplates)
            : this()
        {
            if (elementTemplates != null)
            {
                _rootElementTemplates = elementTemplates;
                SynchUI();
                return;
            }
            else
            {
                throw new ArgumentException("You must specify either an attribute or database.");
            }
        }

        public AttributeSelectionForm(AFElementTemplate elementTemplate)
            : this()
        {
            if (elementTemplate != null)
            {
                _rootElementTemplate = elementTemplate;
                SynchUI();
                return;
            }
            else
            {
                throw new ArgumentException("You must specify either an attribute or database.");
            }
        }

        private void SynchUI()
        {
            try
            {
                if (_rootElement == null)
                {
                    if (_rootElementTemplates == null)
                    {
                        if (_rootElementTemplate == null)
                        {
                            afTreeView1.SetAFRoot(_database.Elements, null, null);
                        }
                        else
                        {
                            AFNamedCollectionList<AFAttributeTemplate> allAttributeTemplates = _rootElementTemplate.GetAllAttributeTemplates();
                            afTreeView1.SetAFRoot(allAttributeTemplates, null, null);
                        }
                    }
                    else
                    {
                        afTreeView1.SetAFRoot(_rootElementTemplates, null, null);
                    }
                }
                else
                {
                    afTreeView1.SetAFRoot(_rootElement, null, null);
                }
                //afTreeView1.AfterSelect(null, null);
            }
            catch (Exception ex)
            {
                AFTrace.TraceEvent(ex);
                DrawingHelper.DisplayErrorMessage(this, null, null, ex);
            }
        }

        private void afTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool defaultAttribute;
            bool flag;
            if (_rootElement != null || _database != null)
            {
                btnOK.Enabled = !AFObject.Equals(SelectedAttribute, null);
                if (_allowElementWithDefaultAttributeSelect && !btnOK.Enabled)
                {
                    Button button = btnOK;
                    if (SelectedElement == null)
                    {
                        defaultAttribute = false;
                    }
                    else
                    {
                        defaultAttribute = SelectedElement.DefaultAttribute != null;
                    }
                    button.Enabled = defaultAttribute;
                    return;
                }
            }
            else
            {
                if (_rootElementTemplate != null || _rootElementTemplates != null)
                {
                    btnOK.Enabled = !AFObject.Equals(SelectedAttributeTemplate, null);
                    if (_allowElementWithDefaultAttributeSelect && !btnOK.Enabled)
                    {
                        Button button = btnOK;
                        if (SelectedElementTemplate == null)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = SelectedElementTemplate.DefaultAttribute != null;
                        }
                        button.Enabled = flag;
                    }
                }
            }
        }

        private void afTreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (btnOK.Enabled)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AttributeSelectionForm_Load(object sender, EventArgs e)
        {

        }

      
       
    }

}
