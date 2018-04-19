using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;

namespace OSIsoft.AF.Asset.DataReference
{
    public partial class TimeRangeEntry : Form
    {
        private MyAttributeTimeRange dataReference = null;

        enum RetrievalMethods { Auto, AtOrBefore, AtOrAfter, Spare1, Exact, Spare2, Before, After, Interpolated, TimeRange, TimeRangeOverride, NotSupported };
        enum Days { day, hour, Microseconds, miliseconds, milliseconds, minute, month, second, week, year, yearsidereal };

        public TimeRangeEntry(MyAttributeTimeRange dataReference, bool bReadOnly)
        {
            InitializeComponent();

            // save for persistence
            this.dataReference = dataReference;
            LoadUI();
        }

        private void LoadUI()
        {
            LoadComboBoxes();
            cmbSourceUnit.Text = dataReference.SourceUnits;
            cmbByTime.Text = dataReference.ByTime;
            cmbByTimeRange.Text = dataReference.TimeRange;
            //txtRelativeTime.Text = txtRelativeTime.Text.Substring(0, txtRelativeTime.Text.Length - 1);
            txtRelativeTime.Text = dataReference.RelativeTime;//.Remove(0,2);
            txtMinPercentGood.Text = dataReference.MinPercentGood.ToString();
            txtTargetAttribute.Text = dataReference.TargetAttributeName;
            cmbCalculationBasis.Text = dataReference.Calculation;

        }

        private void LoadComboBoxes()
        {

            cmbSourceUnit.InitializeObject(new PISystems().DefaultPISystem.UOMDatabase);
            cmbByTime.DataSource = Enum.GetNames(typeof(RetrievalMethods));
            cmbCalculationBasis.DataSource = Enum.GetValues(typeof(AFCalculationBasis));
            cmbByTimeRange.DataSource = Enum.GetValues(typeof(AFSummaryTypes));
            cmbDay.DataSource = Enum.GetNames(typeof(Days));

        }

        private List<string> FindChildElementTemplates()
        {
            List<string> _templateNames = new List<string>();
            try
            {
                if (dataReference.Attribute != null)
                {
                    AFElement element = (AFElement)dataReference.Attribute.Element;
                    AFNamedCollectionList<AFElement> childElements = AFElement.FindElements(element.Database, element, "*",
                        AFSearchField.Name, false, AFSortField.Name, AFSortOrder.Ascending, 1000000);

                    if (childElements != null && childElements.Count > 0)
                    {
                        foreach (AFElement childElement in childElements)
                        {
                            if (!(_templateNames.Contains(childElement.Template.Name)))
                            {
                                _templateNames.Add(childElement.Template.Name);
                            }
                        }
                    }
                }

                if (dataReference.Template != null)
                {
                    foreach (AFElementTemplate template in dataReference.Database.ElementTemplates)
                    {
                        _templateNames.Add(template.Name);
                    }
                }

                _templateNames.Sort();
                return _templateNames;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Scrapes the UI to return configuration values back to the DR
        /// </summary>
        private bool GetValuesFromForm()
        {
            try
            {
                dataReference.TargetAttributeName = txtTargetAttribute.Text;
                dataReference.SourceUnits = cmbSourceUnit.Text;
                dataReference.ByTime = cmbByTime.Text;
                dataReference.RelativeTime = txtRelativeTime.Text;
                dataReference.TimeRange = cmbByTimeRange.Text;
                dataReference.Calculation = cmbCalculationBasis.Text;
                dataReference.MinPercentGood = float.Parse(txtMinPercentGood.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Unable to apply changes: {0}", ex.Message), "Error");
                return false;
            }
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // save the settings
            if (!GetValuesFromForm())
            {
                // don't close if validation error
                DialogResult = DialogResult.None;
            }
        }

        private void btnAttributeSearch_Click(object sender, EventArgs e)
        {
            bool isTemplate = false;
            AttributeSelectionForm dlg = null;
            try
            {
                if (dataReference.Attribute != null)
                {
                    // Configuration is being done from an instantiated element
                    AFElement _rootElement = (AFElement)dataReference.Attribute.Element;
                    dlg = new AttributeSelectionForm(_rootElement);
                }

                if (dataReference.Template != null)
                {
                    // Configuration is being done from an element template, so we show the list of templates in the DB
                    dlg = new AttributeSelectionForm(dataReference.Database.ElementTemplates);
                    isTemplate = true;
                }

                dlg.AllowElementWithDefaultAttributeSet = true;
                dlg.ShowInTaskbar = false;
                DialogResult dr = dlg.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (isTemplate)
                    {
                        txtTargetAttribute.Text = dlg.RelativePath;
                    }
                    else
                    {
                        txtTargetAttribute.Text = dlg.SelectedAttribute.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                dlg.Dispose();
            }
        }

        private void cmbByTimeRange_TextChanged(object sender, EventArgs e)
        {
            if (String.Compare(cmbByTimeRange.Text, "Average") == 0)
            {
                cmbCalculationBasis.Enabled = true;
                //dataReference.Calculation = cmbCalculationBasis.Text;

                txtMinPercentGood.Enabled = true;
                //dataReference.MinPercentGood = float.Parse(txtMinPercentGood.Text);
            }
            else
            {
                cmbCalculationBasis.Enabled = false;
                cmbCalculationBasis.SelectedIndex = -1;

                //cmbCalculationBasis.Text = "";
                txtMinPercentGood.Enabled = false;
            }

            if ((String.Compare(cmbByTimeRange.Text, "Maximum") == 0) || (String.Compare(cmbByTimeRange.Text, "Minimum") == 0))
            {
                cmbCalculationBasis.Enabled = false;
                cmbCalculationBasis.SelectedIndex = -1;
                txtMinPercentGood.Enabled = true;
            }
            else
            {
                cmbCalculationBasis.Enabled = true;
                txtMinPercentGood.Enabled = true;
            }

            if (String.Compare(cmbByTimeRange.Text, "Total") == 0)
            {
                label1.Visible = true;
                cmbDay.Visible = true;
            }
            else
            {
                label1.Visible = false;
                cmbDay.Visible = false;
            }


            if (String.Compare(cmbByTimeRange.Text, " ") == 0)
            {
                //cmbCalculationBasis.SelectedIndex = -1;
                txtMinPercentGood.Text = String.Empty;
            }
            else
            {


            }

        }

        private void cmbByTime_TextChanged(object sender, EventArgs e)
        {
            if (String.Compare(cmbByTime.Text, "NotSupported") == 0)
            {
                txtRelativeTime.Enabled = false;
                txtRelativeTime.Text = String.Empty;

            }
            else
            {
                txtRelativeTime.Enabled = true;

            }
        }

        private void cmbCalculationBasis_EnabledChanged(object sender, EventArgs e)
        {
            MessageBox.Show("This button has been enabled.");
        }

       
    }
}
