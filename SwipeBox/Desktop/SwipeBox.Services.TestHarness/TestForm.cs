﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwipeBox.Services.TestHarness
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var clientId = Convert.ToInt32(textBox1.Text);
            

            using (var client = new SwipeBoxServiceReference.SwipeBoxServiceClient())
            {
                var retVal = client.AddMeeting(clientId);

                MessageBox.Show("Added Successfully: " +retVal);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var email = (emailText.Text);

            try
            {
                using (var client = new SwipeBoxServiceReference.SwipeBoxServiceClient())
                {
                    var retVal = client.GetClientByEmail(email);

                    MessageBox.Show("Found: " + retVal.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
