﻿using LibraryMatrix;
using LibraryMatrix.core;
using LibraryMatrix.implementations;
using LibraryMatrix.interfaces;
using LibraryMatrix.operations;
using Label = System.Windows.Forms.Label;

namespace CalcMatrix
{
    public partial class FormDetermCharactNumb : Form
    {
        public TextBoxMatrix textBoxMatrix;


        List<Label> resultLabels = new List<Label>();

        int explanationMargin = 20;
        int labelX = 80;
        int labelWidth = 350;
        int currentY = 400;
        public FormDetermCharactNumb()
        {
            InitializeComponent();
        }

        private void buttonDisplayTextBox_Click(object sender, EventArgs e)
        {
            int rows = (int)numericUpDownRowsMatrix.Value;
            int cols = (int)numericUpDownColMatrix.Value;

            IDataInputFactory dataInputFactory = new WinFormDataInputFactory();
            textBoxMatrix = new TextBoxMatrix(rows, cols, 80, 250, dataInputFactory);

            foreach (var dataInput in textBoxMatrix.DataInputs)
            {
                Controls.Add(((WinFormTextBox)dataInput).GetTextBox());
            }
        }
        private void buttonDetermSol_Click(object sender, EventArgs e)
        {
            double[,] matrixValues = MatrixProcessor.GetMatrixValues(textBoxMatrix);
            if (matrixValues != null)
            {
                int rows = textBoxMatrix.Rows;
                int cols = textBoxMatrix.Columns;
                IMatrix matrix = new Matrix(rows, cols, matrixValues);
                ClearResultLabels();
                double determinant = 0.0;
                if (checkBoxDeterm.Checked)
                {
                    if (rows == 1 && cols == 1 || rows == 2 && cols == 2 || (rows == 3 && cols == 3))
                    {
                        if (rows == 1 && cols == 1 || rows == 2 && cols == 2)
                        {
                            determinant = MatrixOperationDetermContext.CalculateDeterminant(matrix, new CalculateDeterminantTriangleMethod());
                            AddResultLabel("Детермінант: " + determinant.ToString());
                        }
                        else if (rows == 3 && cols == 3)
                        {
                            if (radioButtonTriangle.Checked)
                            {
                                determinant = MatrixOperationDetermContext.CalculateDeterminant(matrix, new CalculateDeterminantTriangleMethod());
                                AddResultLabel("Детермінант: " + determinant.ToString());
                            }
                            else if (radioButtonSar.Checked)
                            {
                                determinant = MatrixOperationDetermContext.CalculateDeterminant(matrix, new CalculateDeterminantSarrus());
                                AddResultLabel("Детермінант: " + determinant.ToString());
                            }
                            else if (radioButtonRoz.Checked)
                            {
                                determinant = MatrixOperationDetermContext.CalculateDeterminant(matrix, new CalculateDeterminantGauss());
                                AddResultLabel($"Детермінант: " + determinant.ToString());
                            }
                        }
                    }
                    else
                    {
                        determinant = MatrixOperationDetermContext.CalculateDeterminant(matrix, new CalculateDeterminantGauss());
                        AddResultLabel($"Детермінант: " + determinant.ToString());
                    }
                }
                if (checkBoxRank.Checked)
                {
                    int rank = MatrixOperationOtherContext.CalculateRank(matrix);
                    AddResultLabel("Ранг: " + rank.ToString());
                }

                if (checkBoxShall.Checked)
                {
                    double trace = MatrixOperationOtherContext.CalculateTrace(matrix);
                    AddResultLabel($"Слід матриці: {trace}");
                }

                if (checkBoxMinelem.Checked)
                {
                    double minimumElement = MatrixOperationOtherContext.FindMinimumElement(matrix);
                    AddResultLabel($"Мінімальний елемент: {minimumElement}");
                }

                if (checkBoxMaxElem.Checked)
                {
                    double maximumElement = MatrixOperationOtherContext.FindMaximumElement(matrix);
                    AddResultLabel($"Максимальний елемент: {maximumElement}");
                }

                if (checkBoxNorm.Checked)
                {
                    double normal = MatrixOperationOtherContext.CalculateMatrixNorm(matrix);
                    AddResultLabel($"Норма матриці: {normal}");
                }

                if (checkBoxAverage.Checked)
                {
                    double average = MatrixOperationOtherContext.CalculateAverage(matrix);
                    AddResultLabel($"Середнє значення: {average}");
                }

                if (checkBoxSum.Checked)
                {
                    double sum = MatrixOperationOtherContext.CalculateSum(matrix);
                    AddResultLabel($"Сума елементів: {sum}");
                }

                if (checkBoxProd.Checked)
                {
                    double product = MatrixOperationOtherContext.CalculateProduct(matrix);
                    AddResultLabel($"Добуток елементів: {product}");
                }
            }
            else
            {
                MessageBox.Show("Не правильно введені дані, перевірте та повторіть спробу ще раз!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void AddResultLabel(string text, string explanation = "")
        {
            Label labelResult = new Label
            {
                Font = new Font("Times New Roman", 12),
                AutoSize = true,
                Text = text,
                Location = new Point(800, resultLabels.Count * 20 + 300)
            };
            resultLabels.Add(labelResult);
            this.Controls.Add(labelResult);

            if (!string.IsNullOrEmpty(explanation))
            {
                Label labelExplanation = new Label
                {
                    AutoSize = true,
                    TextAlign = ContentAlignment.TopLeft,
                    Location = new Point(labelX, resultLabels.Count * 40 + 450),
                    Width = labelWidth,
                    Font = new Font("Times New Roman", 12),
                    Text = explanation
                };
                resultLabels.Add(labelExplanation);
                this.Controls.Add(labelExplanation);
            }
        }
        private void ClearResultLabels()
        {
            foreach (Label label in resultLabels)
            {
                this.Controls.Remove(label);
                label.Dispose();
            }
            resultLabels = new List<Label>();
        }
        private void FormDetermCharactNumb_Load(object sender, EventArgs e)
        {

        }

        private void buttonClear1_Click(object sender, EventArgs e)
        {
            if (textBoxMatrix != null)
            {
                foreach (var dataInput in textBoxMatrix.DataInputs)
                {
                    var textBox = ((WinFormTextBox)dataInput).GetTextBox();
                    if (textBox.Parent == this)
                    {
                        Controls.Remove(textBox);
                        textBox.Dispose();
                    }
                }
            }
        }
    }
}
