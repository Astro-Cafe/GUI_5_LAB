﻿using Chat_Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Chat_Client.ViewModel
{
	public class MainWindowViewModel : ObservableRecipient
	{
		private string Sender { get; set; } = "";

		private string inputText = "";

		public string InputText
		{
			get { return inputText; }
			set { inputText = value; OnPropertyChanged(); (SendCommand as RelayCommand).NotifyCanExecuteChanged(); }
		}

		public RestCollection<Message> Messages { get; set; }

		public ICommand SendCommand { get; set; }

		public static bool IsInDesignMode
		{
			get
			{
				var prop = DesignerProperties.IsInDesignModeProperty;
				return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
			}
		}

		public MainWindowViewModel()
		{
			if (!IsInDesignMode)
			{
				Messages = new RestCollection<Message>("http://localhost:31021/", "message", "hub");
				SendCommand = new RelayCommand(() => {
					Messages.Add(new Message() { Sender = this.Sender, SentMessage = InputText });
					InputText = "";
				}, () => { return InputText != null && InputText != ""; });

				// Hidden dependency, sorry, quick solution, should be done /w IoC :) :)))
				//NameService = new NameServiceViaWindow();

				// Collection change event forward to code behind to scrollbar adjustment
				//Messages.CollectionChanged += Messages_CollectionChanged;

				// Window Loaded event receiver
				Messenger.Register<object, string, string>(this, "MainWindowLoaded", (recipient, msg) =>
				{
					this.Sender = NameService.GetName();
				});
			}
		}
		private void Messages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this.Messenger.Send("Changed", "ChatChanged");
		}
	}
}
