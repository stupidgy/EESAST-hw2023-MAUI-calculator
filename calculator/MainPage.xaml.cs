﻿using System;
using appdata;
using calculator_plus;
using static System.Net.Mime.MediaTypeNames;

namespace calculator;

public partial class MainPage : ContentPage
{
    // 使用构造函数来初始化页面
    public MainPage()
    {
        // 调用InitializeComponent方法来加载XAML文件中定义的控件
        InitializeComponent();

        AppData.Instance.TextUpdated += AppData_TextUpdated;
    }



    // 定义OnNumberClicked方法来处理数字按钮点击事件
    private void OnNumberClicked(object sender, EventArgs e)
    {
        // 获取按钮的文本值
        var button = sender as Button;
        var number = button.Text;
        AppData.Instance.iscurrentNumber = true;
    
        // 如果当前显示的是结果，或者是0，就清空显示屏
        if (AppData.Instance.isResult || displayLabel.Text == "0")
        {
            displayLabel.Text = "";
            if (number == ".")
                displayLabel.Text = "0";
            AppData.Instance.isResult = false;
        }

        //输入..会崩溃
        if (!(number == "." && displayLabel.Text.Contains(".")))
        {
            // 将数字追加到显示屏，并更新当前输入的数字
            displayLabel.Text += number;
        }
        AppData.Instance.currentNumber = double.Parse(displayLabel.Text);
    }

    // 定义OnOperatorClicked方法来处理运算符按钮点击事件
    private void OnOperatorClicked(object sender, EventArgs e)
    {
        // 获取按钮的文本值
        var button = sender as Button;
        var op = button.Text;

        AppData.Instance.lastNumber = AppData.Instance.currentNumber;
        displayLabel.Text = "0";
        AppData.Instance.isResult = false;
        AppData.Instance.iscurrentNumber = false;

        // 将当前选择的运算符赋值给变量，并清空当前输入的数字
        AppData.Instance.currentOperator = op;
    }

    // 定义OnEqualClicked方法来处理等号按钮点击事件
    private void OnEqualClicked(object sender, EventArgs e)
    {
        // 如果当前选择的运算符不为空，就执行上一次选择的运算，并显示结果
        if (AppData.Instance.currentOperator != "")
        {
            Calculate();
            displayLabel.Text = AppData.Instance.lastNumber.ToString();
            AppData.Instance.isResult = true;
            AppData.Instance.currentOperator = "";
        }
    }

    // 定义OnClearClicked方法来处理AC按钮点击事件
    private void OnClearClicked(object sender, EventArgs e)
    {
        AppData.Instance.currentNumber = 0;
        AppData.Instance.lastNumber = 0;
        AppData.Instance.currentOperator = "";
        AppData.Instance.isResult = false;
        AppData.Instance.iscurrentNumber = false;
        displayLabel.Text = AppData.Instance.lastNumber.ToString();
    }

    //定义OnDELClicked方法来处理DEL按钮点击事件
    private void OnDELClicked(object sender, EventArgs e)
    {
        if (!AppData.Instance.isResult)
        {
            if (AppData.Instance.iscurrentNumber == false)
            {
                AppData.Instance.currentOperator = "";
                AppData.Instance.iscurrentNumber = true;
                displayLabel.Text = AppData.Instance.lastNumber.ToString();
            }
            else
            {
                string text = displayLabel.Text;
                if (text.Length > 1)
                {
                    double tempCurrentNumber;
                    double.TryParse(text.Substring(0, text.Length - 1),out tempCurrentNumber);
                    AppData.Instance.currentNumber = tempCurrentNumber;
                    displayLabel.Text = AppData.Instance.currentNumber.ToString();
                    if (text[text.Length - 2] == '.')
                    {
                        displayLabel.Text += ".";
                    }
                }
                else
                {
                    AppData.Instance.currentNumber = 0;
                    if (AppData.Instance.currentOperator != "")
                    {
                        AppData.Instance.iscurrentNumber = false;
                    }
                    displayLabel.Text = "";
                }
            }
        }
        else
        {
            displayLabel.Text = "";
        }
    }

    //定义OnXClicked方法来处理X按钮点击事件
    private void OnXClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewPage(displayLabel.Text));
        
    }

    private void AppData_TextUpdated(object sender, string newText)
    {
        // 在事件处理程序中更新文本框内容
        displayLabel.Text = newText;
    }


    // 定义Calculate方法来执行运算逻辑
    private void Calculate()
    {
        // 根据当前选择的运算符，对上一次计算的结果和当前输入的数字进行相应的运算，并更新上一次计算的结果
        switch (AppData.Instance.currentOperator)
        {
            case "+":
                AppData.Instance.lastNumber += AppData.Instance.currentNumber;
                break;
            case "-":
                AppData.Instance.lastNumber -= AppData.Instance.currentNumber;
                break;
            case "*":
                AppData.Instance.lastNumber *= AppData.Instance.currentNumber;
                break;
            case "/":
                AppData.Instance.lastNumber /= AppData.Instance.currentNumber;
                break;
            case "^":
                AppData.Instance.lastNumber = Math.Pow(AppData.Instance.lastNumber, AppData.Instance.currentNumber);
                break;
            case "!":
                int factorial = 1;
                for (int i = 1; i <= AppData.Instance.lastNumber; i++)
                {
                    factorial *= i;
                }
                AppData.Instance.lastNumber = factorial;
                break;
            case "lg":
                AppData.Instance.lastNumber = Math.Log10(AppData.Instance.currentNumber);
                break;
            case "ln":
                AppData.Instance.lastNumber = Math.Log(AppData.Instance.currentNumber);
                break;
            case "√":
                AppData.Instance.lastNumber = Math.Sqrt(AppData.Instance.currentNumber);
                break;
            case "sin":
                AppData.Instance.lastNumber = Math.Sin(AppData.Instance.currentNumber);
                break;
            case "cos":
                AppData.Instance.lastNumber = Math.Cos(AppData.Instance.currentNumber);
                break;
            case "tan":
                AppData.Instance.lastNumber = Math.Tan(AppData.Instance.currentNumber);
                break;
            default:
                break;
        }
        if (AppData.Instance.iscurrentNumber == false && AppData.Instance.currentOperator != "!")
        {
            AppData.Instance.lastNumber = AppData.Instance.currentNumber;
        }
        AppData.Instance.lastNumber = Math.Round(AppData.Instance.lastNumber, 4);
        AppData.Instance.currentNumber = AppData.Instance.lastNumber;
    }
}


