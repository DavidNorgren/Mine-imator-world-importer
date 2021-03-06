﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace import
{
	public partial class frmFilters : Form
	{
		// Represents a chooseable block variation
		class BlockOption
		{
			public string name, displayName;

			public BlockOption(string name, string displayName)
			{
				this.name = name;
				this.displayName = displayName;
			}

			public override string ToString()
			{
				return displayName;
			}
		}

		frmImport import;
		JavaScriptSerializer serializer = new JavaScriptSerializer();

		public frmFilters(Form callingForm)
		{
			import = callingForm as frmImport;
			InitializeComponent();
		}

		private void frmFilters_Load(object sender, EventArgs e)
		{
			// Set texts
			Text = import.GetText("filterstitle");
			cbxActivate.Text = import.GetText("filtersactivate");
			lblBlocksToRemove.Text = import.GetText("filtersblockstoremove");
			rbtnRemoveBlocks.Text = import.GetText("filtersremovefiltered");
			rbtnKeepFiltered.Text = import.GetText("filterskeepfiltered");
			rbtnKeepFiltered.Location = new Point(rbtnRemoveBlocks.Location.X + rbtnRemoveBlocks.Width + 20, rbtnKeepFiltered.Location.Y);
			btnAdd.Text = import.GetText("filtersadd");
			btnRemove.Text = import.GetText("filtersremove");
			btnOk.Text = import.GetText("filtersok");

			LoadFilters();
		}

		private void LoadFilters()
		{
			// Load block variants and store in combobox and listbox
			foreach (KeyValuePair<string, frmImport.Block> pair in import.blockNameMap)
			{ 
				BlockOption option = new BlockOption(pair.Key, pair.Value.displayName);
				cbxBlocks.Items.Add(option);
				if (import.filterBlockNames.Contains(pair.Key))
					lbxFilters.Items.Add(option);
			}

			// Settings
			cbxActivate.Checked = import.filterBlocksActive;
			rbtnRemoveBlocks.Checked = !import.filterBlocksInvert;
			rbtnKeepFiltered.Checked = import.filterBlocksInvert;
			panFilters.Enabled = cbxActivate.Checked;
		}

		private void cbxActivate_CheckedChanged(object sender, EventArgs e)
		{
			panFilters.Enabled = cbxActivate.Checked;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (cbxBlocks.SelectedIndex == -1 || cbxBlocks.Text == "")
				return;

			// Already added in list
			BlockOption option = (BlockOption)cbxBlocks.SelectedItem;
			if (import.filterBlockNames.Contains(option.name))
				return;

			import.filterBlockNames.Add(option.name);
			lbxFilters.Items.Add(option);
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			BlockOption option = (BlockOption)lbxFilters.SelectedItem;
			import.filterBlockNames.Remove(option.name);
			lbxFilters.Items.Remove(option);
			btnRemove.Enabled = false;
		}


		private void btnOk_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void frmFilters_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Save filtered blocks to JSON
			string json = "{\n";
			json += "\t\"active\": " + (cbxActivate.Checked ? "true" : "false") + ",\n";
			json += "\t\"invert\": " + (rbtnKeepFiltered.Checked ? "true" : "false") + ",\n";
			json += "\t\"blocks\": [\n";
			for (int i = 0; i < import.filterBlockNames.Count; i++)
				json += "\t\t\"" + import.filterBlockNames[i] + "\"" + (i < import.filterBlockNames.Count - 1 ? "," : "" ) + "\n";
			json += "\t]\n";
			json += "}";

			File.WriteAllText(frmImport.miBlockFilterFile, json);

			import.filterBlocksActive = cbxActivate.Checked;
			import.filterBlocksInvert = rbtnKeepFiltered.Checked;
			import.UpdateFilterBlocks();
		}

		private void rbtnRemoveFiltered_CheckedChanged(object sender, EventArgs e)
		{
			lblBlocksToRemove.Text = import.GetText("filtersblockstoremove");
		}

		private void rbtnKeepFiltered_CheckedChanged(object sender, EventArgs e)
		{
			lblBlocksToRemove.Text = import.GetText("filtersblockstokeep");
		}

		private void cbxBlocks_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnAdd.Enabled = (cbxBlocks.SelectedItem != null);
		}

		private void lbxFilters_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnRemove.Enabled = (lbxFilters.SelectedItem != null);
		}
	}
}
