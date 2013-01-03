using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;
using GalaSoft.MvvmLight.Messaging;
using CountSmart.Messages;
using System.Windows;

namespace CountSmart.ViewModel
{
    public class CounterViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Value" /> property's name.
        /// </summary>
        public const string ValuePropertyName = "Value";

        private int _value = 0;
        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (_value == value)
                {
                    return;
                }

                var oldValue = _value;
                _value = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(ValuePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Step" /> property's name.
        /// </summary>
        public const string StepPropertyName = "Step";

        private int _step = 1;
        public int Step
        {
            get
            {
                return _step;
            }

            set
            {
                if (_step == value)
                {
                    return;
                }

                var oldValue = _step;
                _step = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(StepPropertyName);
            }
        }
        

        public CounterViewModel()
        {
            if (IsInDesignMode)
            {
                Value = 100;

            }
            else
            {
                // Code runs "for real": Connect to service, etc...
            }

            this.RegisterMessages();
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<ClearCounterValueMessage>(this, OnClearCounterValueMessage);
            Messenger.Default.Register<SendCounterValueAsSmsMessage>(this, OnSendCounterValueAsSmsMessage);
            Messenger.Default.Register<SendCounterValueAsEmailMessage>(this, OnSendCounterValueAsEmailMessage);
        }

        private void OnClearCounterValueMessage(ClearCounterValueMessage msg)
        {
            this.ClearValueExecute();
        }

        private void OnSendCounterValueAsSmsMessage(SendCounterValueAsSmsMessage msg)
        {
            this.SendBySmsExecute();
        }

        private void OnSendCounterValueAsEmailMessage(SendCounterValueAsEmailMessage msg)
        {
            this.SendByEmailExecute();
        }

        #region IncrementCommand

        private RelayCommand _incrementCommand;
        public RelayCommand IncrementCommand
        {
            get
            {
                if (_incrementCommand == null)
                {
                    _incrementCommand = new RelayCommand(
                            () =>
                            {
                                IncrementExecute();
                            },
                            () => CanIncrement
                        );
                }
                return _incrementCommand;
            }
            set
            {
                _incrementCommand = value;
            }
        }
        public void IncrementExecute()
        {
            this.Value += Step;
        }

        public const string CanIncrementPropertyName = "CanIncrement";
        private bool _canIncrement = true;
        public bool CanIncrement
        {
            get
            {
                return _canIncrement;
            }

            set
            {
                if (_canIncrement == value)
                {
                    return;
                }

                var oldValue = _canIncrement;
                _canIncrement = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanIncrementPropertyName);

                //tells the controls that are binded to the Command if it can execute
                IncrementCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region DecrementCommand

        private RelayCommand _decrementCommand;
        public RelayCommand DecrementCommand
        {
            get
            {
                if (_decrementCommand == null)
                {
                    _decrementCommand = new RelayCommand(
                            () =>
                            {
                                DecrementExecute();
                            },
                            () => CanDecrement
                        );
                }
                return _decrementCommand;
            }
            set
            {
                _decrementCommand = value;
            }
        }

        private const string TEXT_COUNT_EMAIL_BODY = "I have just counted {0} ... \n with #CountSmart";
        private const string TEXT_COUNT_EMAIL_SUBJECT = "Counting result";
        private const string TEXT_COUNT_SMS_BODY = "I have just counted {0} ...";
    
        public void DecrementExecute()
        {
            this.Value -= Step;
        }

        public const string CanDecrementPropertyName = "CanDecrement";
        private bool _canDecrement = true;
        public bool CanDecrement
        {
            get
            {
                return _canDecrement;
            }

            set
            {
                if (_canDecrement == value)
                {
                    return;
                }

                var oldValue = _canDecrement;
                _canDecrement = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanDecrementPropertyName);

                //tells the controls that are binded to the Command if it can execute
                DecrementCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region SendByEmailCommand

        private RelayCommand _sendByEmailCommand;
        public RelayCommand SendByEmailCommand
        {
            get
            {
                if (_sendByEmailCommand == null)
                {
                    _sendByEmailCommand = new RelayCommand(
                            () =>
                            {
                                SendByEmailExecute();
                            },
                            () => CanSendByEmail
                        );
                }
                return _sendByEmailCommand;
            }
            set
            {
                _sendByEmailCommand = value;
            }
        }

        public void SendByEmailExecute()
        {
            int counterValue = Value;
            this.SendEmail(counterValue);
        }

        private void SendEmail(int counterValue)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = TEXT_COUNT_EMAIL_SUBJECT;
            emailComposeTask.Body = string.Format(TEXT_COUNT_EMAIL_BODY, counterValue);
            emailComposeTask.Show();
        }


        public const string CanSendByEmailPropertyName = "CanSendByEmail";

        private bool _canSendByEmail = true;
        public bool CanSendByEmail
        {
            get
            {
                return _canSendByEmail;
            }

            set
            {
                if (_canSendByEmail == value)
                {
                    return;
                }

                var oldValue = _canSendByEmail;
                _canSendByEmail = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanSendByEmailPropertyName);

                //tells the controls that are binded to the Command if it can execute
                SendByEmailCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region SendBySmsCommand

        private RelayCommand _sendBySmsCommand;
        public RelayCommand SendBySmsCommand
        {
            get
            {
                if (_sendBySmsCommand == null)
                {
                    _sendBySmsCommand = new RelayCommand(
                            () =>
                            {
                                SendBySmsExecute();
                            },
                            () => CanSendBySms
                        );
                }
                return _sendBySmsCommand;
            }
            set
            {
                _sendBySmsCommand = value;
            }
        }
        public void SendBySmsExecute()
        {
            int counterValue = Value;
            this.SendSms(counterValue);
        }

        private void SendSms(int counterValue)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.Body = string.Format(TEXT_COUNT_SMS_BODY, counterValue); ;
            smsComposeTask.Show();
        }

        public const string CanSendBySmsPropertyName = "CanSendBySms";

        private bool _canSendBySms = true;
        public bool CanSendBySms
        {
            get
            {
                return _canSendBySms;
            }

            set
            {
                if (_canSendBySms == value)
                {
                    return;
                }

                var oldValue = _canSendBySms;
                _canSendBySms = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanSendBySmsPropertyName);

                //tells the controls that are binded to the Command if it can execute
                SendBySmsCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region ClearValueCommand

        private RelayCommand _clearValueCommand;
        public RelayCommand ClearValueCommand
        {
            get
            {
                if (_clearValueCommand == null)
                {
                    _clearValueCommand = new RelayCommand(
                            () =>
                            {
                                ClearValueExecute();
                            },
                            () => CanClearValue
                        );
                }
                return _clearValueCommand;
            }
            set
            {
                _clearValueCommand = value;
            }
        }


        public void ClearValueExecute()
        {
            if (MessageBox.Show("Clear current value?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.ClearValue();
            }
        }

        private void ClearValue()
        {
            this.Value = 0;
        }
        public const string CanClearValuePropertyName = "CanClearValue";

        private bool _canClearValue = true;
        public bool CanClearValue
        {
            get
            {
                return _canClearValue;
            }

            set
            {
                if (_canClearValue == value)
                {
                    return;
                }

                var oldValue = _canClearValue;
                _canClearValue = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CanClearValuePropertyName);

                //tells the controls that are binded to the Command if it can execute
                ClearValueCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}