﻿// Copyright 2012-2013 Alalf <alalf.iQLc_at_gmail.com>
//
// This file is part of SCFF-DirectShow-Filter(SCFF DSF).
//
// SCFF DSF is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SCFF DSF is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SCFF DSF.  If not, see <http://www.gnu.org/licenses/>.

/// @file SCFF.GUI/MainWindow.xaml.cs
/// @copydoc SCFF::GUI::MainWindow

namespace SCFF.GUI {

using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Microsoft.Windows.Shell;
using SCFF.Common;
using SCFF.Common.GUI;

/// MainWindowのコードビハインド
public partial class MainWindow
    : Window, IBindingProfile, IBindingOptions, IBindingRuntimeOptions {
  //===================================================================
  // コンストラクタ/Dispose/デストラクタ
  //===================================================================

  /// コンストラクタ
  public MainWindow() {
    this.InitializeComponent();

    this.OnOptionsChanged();
    this.OnRuntimeOptionsChanged();
    this.OnProfileChanged();

    // 必要な機能の実行
    this.SetAero();
    this.SetCompactView(false);
  }

  //===================================================================
  // イベントハンドラ
  //===================================================================

  /// アプリケーション終了時に発生するClosingイベントハンドラ
  protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
    base.OnClosing(e);

    this.SaveTemporaryOptions();
  }

  /// Deactivated
  /// @param e 使用しない
  protected override void OnDeactivated(System.EventArgs e) {
    base.OnDeactivated(e);

    /// @todo(me) スクリーンキャプチャをの更新頻度を下げる
    ///           App.RuntimeOptionsに該当するデータを保存しておく感じかな？
    // Debug.WriteLine("Deactivated", "MainWindow");
  }

  /// Activated
  /// @param e 使用しない
  protected override void OnActivated(System.EventArgs e) {
    base.OnActivated(e);

    /// @todo(me) スクリーンキャプチャを更新頻度を元に戻す
    ///           App.RuntimeOptionsに該当するデータを保存しておく感じかな？
    // Debug.WriteLine("Activated", "MainWindow");
  }

  //-------------------------------------------------------------------
  // *Changed/Checked/Unchecked以外
  //-------------------------------------------------------------------

  //-------------------------------------------------------------------
  // Checked/Unchecked
  //-------------------------------------------------------------------

  //-------------------------------------------------------------------
  // *Changed/Collapsed/Expanded
  //-------------------------------------------------------------------

  /// AreaExpander: Collapsed
  private void AreaExpander_Collapsed(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.AreaIsExpanded = false;
  }
  /// AreaExpander: Expanded
  private void AreaExpander_Expanded(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.AreaIsExpanded = true;
  }
  /// OptionsExpander: Collapsed
  private void OptionsExpander_Collapsed(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.OptionsIsExpanded = false;
  }
  /// OptionsExpander: Expanded
  private void OptionsExpander_Expanded(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.OptionsIsExpanded = true;
  }
  /// ResizeMethodExpander: Collapsed
  private void ResizeMethodExpander_Collapsed(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.ResizeMethodIsExpanded = false;
  }
  /// ResizeMethodExpander: Expanded
  private void ResizeMethodExpander_Expanded(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.ResizeMethodIsExpanded = true;
  }
  /// LayoutExpander: Collapsed
  private void LayoutExpander_Collapsed(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.LayoutIsExpanded = false;

    //-----------------------------------------------------------------
    // Notify self
    this.FixMinMaxWidth();
    this.FixWidthAndHeight();
    // Notify other controls
    Commands.ProfileVisualChanged.Execute(null, null);
    //-----------------------------------------------------------------
  }
  /// LayoutExpander: Expanded
  private void LayoutExpander_Expanded(object sender, RoutedEventArgs e) {
    if (!this.CanChangeOptions) return;
    App.Options.LayoutIsExpanded = true;

    //-----------------------------------------------------------------
    // Notify self
    this.FixMinMaxWidth();
    this.FixWidthAndHeight();
    // Notify other controls
    Commands.ProfileVisualChanged.Execute(null, null);
    //-----------------------------------------------------------------
  }

  //===================================================================
  // IBindingOptionsの実装
  //===================================================================

  /// AeroをON/OFF
  private void SetAero() {
    if (!this.CanUseAero()) return;
    if (App.Options.ForceAeroOn) {
      // @todo(me) 実装
    } else {
      // @todo(me) 実装
    }
  }

  /// AeroのON/OFFが可能か
  private bool CanUseAero() {
    // @todo(me) 実装
    return true;
  }

  /// コンパクト表示切替
  private void SetCompactView(bool fixValues) {
    if (fixValues) {
      this.FixMinMaxWidth();
      this.FixWidthAndHeight();
    }
    if (App.Options.CompactView) {
      this.OptionsExpander.Visibility = Visibility.Collapsed;
      this.ResizeMethodExpander.Visibility = Visibility.Collapsed;
      this.LayoutExpander.IsExpanded = false;
    } else {
      this.OptionsExpander.Visibility = Visibility.Visible;
      this.ResizeMethodExpander.Visibility = Visibility.Visible;
    }
  }

  /// Width/Heightの設定
  private void FixWidthAndHeight() {
    if (App.Options.CompactView) {
      this.Width = Constants.CompactMainWindowWidth;
      this.Height = Constants.CompactMainWindowHeight;
    } else {
      this.Width = App.Options.TmpMainWindowWidth;
      this.Height = App.Options.TmpMainWindowHeight;
    }
  }

  /// Max/MinWidthの設定
  private void FixMinMaxWidth() {
    if (App.Options.LayoutIsExpanded) {
      this.MinWidth = Constants.MainWindowMinWidthWithLayoutEdit;
      this.MaxWidth = double.PositiveInfinity;
    } else {
      this.MinWidth = Constants.CompactMainWindowWidth;
      this.MaxWidth = Constants.CompactMainWindowWidth;
    }
  }

  //-------------------------------------------------------------------

  /// @copydoc Common::GUI::IBindingOptions::CanChangeOptions
  public bool CanChangeOptions { get; private set; }
  /// @copydoc Common::GUI::IBindingOptions::OnOptionsChanged
  public void OnOptionsChanged() {
    this.CanChangeOptions = false;

    // Temporary
    this.Left         = App.Options.TmpMainWindowLeft;
    this.Top          = App.Options.TmpMainWindowTop;
    this.FixMinMaxWidth();
    this.WindowState  = (System.Windows.WindowState)App.Options.TmpMainWindowState;
    
    // MainWindow.Controls
    this.AreaExpander.IsExpanded          = App.Options.AreaIsExpanded;
    this.OptionsExpander.IsExpanded       = App.Options.OptionsIsExpanded;
    this.ResizeMethodExpander.IsExpanded  = App.Options.ResizeMethodIsExpanded;
    this.LayoutExpander.IsExpanded        = App.Options.LayoutIsExpanded;

    this.FixWidthAndHeight();

    // UserControls
    this.Apply.OnOptionsChanged();
    this.LayoutToolbar.OnOptionsChanged();
    this.LayoutEdit.OnOptionsChanged();
    this.MainMenu.OnOptionsChanged();

    this.CanChangeOptions = true;
  }

  /// UIから設定にデータを保存
  private void SaveTemporaryOptions() {
    // Tmp接頭辞のプロパティだけはここで更新する必要がある
    var isNormal = this.WindowState == System.Windows.WindowState.Normal;
    App.Options.TmpMainWindowLeft = isNormal ? this.Left : this.RestoreBounds.Left;
    App.Options.TmpMainWindowTop = isNormal ? this.Top : this.RestoreBounds.Top;
    if (App.Options.CompactView) {
      // nop
    } else {
      App.Options.TmpMainWindowWidth = isNormal ? this.Width : this.RestoreBounds.Width;
      App.Options.TmpMainWindowHeight = isNormal ? this.Height : this.RestoreBounds.Height;
    }
    Debug.WriteLine("Options WindowSize: {0}x{1}", App.Options.TmpMainWindowWidth, App.Options.TmpMainWindowHeight);
    App.Options.TmpMainWindowState = (SCFF.Common.WindowState)this.WindowState;
  }

  //===================================================================
  // IBindingRuntimeOptionsの実装
  //===================================================================

  /// @copydoc Common::GUI::IBindingRuntimeOptions::CanChangeRuntimeOptions
  public bool CanChangeRuntimeOptions { get; private set; }
  /// @copydoc Common::GUI::IBindingRuntimeOptions::OnRuntimeOptionsChanged
  public void OnRuntimeOptionsChanged() {
    this.CanChangeRuntimeOptions = false;
    /// @todo System.Reflection.Assembly.GetExecutingAssembly().GetName().Versionを使うか？
    ///       しかしどう見てもこれ実行時に決まる値で気持ち悪いな・・・
    var commonTitle = "SCFF DirectShow Filter Ver.0.1.7";
    if (App.RuntimeOptions.ProfilePath != string.Empty) {
      var profileName = Path.GetFileNameWithoutExtension(App.RuntimeOptions.ProfilePath);
      this.WindowTitle.Content = string.Format("{0} - {1}", profileName, commonTitle);
    } else {
      this.WindowTitle.Content = commonTitle;
    }

    this.LayoutEdit.OnRuntimeOptionsChanged();
    this.LayoutParameter.OnRuntimeOptionsChanged();
    this.SCFFEntries.OnRuntimeOptionsChanged();
    this.CanChangeRuntimeOptions = true;
  }

  //===================================================================
  // IBindingProfileの実装
  //===================================================================

  /// @copydoc Common::GUI::IBindingProfile::CanChangeProfile
  public bool CanChangeProfile { get; private set; }

  /// @copydoc Common::GUI::IBindingProfile::OnCurrentLayoutElementChanged
  public void OnCurrentLayoutElementChanged() {
    this.CanChangeProfile = false;
    this.TargetWindow.OnCurrentLayoutElementChanged();
    this.Area.OnCurrentLayoutElementChanged();
    this.Options.OnCurrentLayoutElementChanged();
    this.ResizeMethod.OnCurrentLayoutElementChanged();
    this.LayoutParameter.OnCurrentLayoutElementChanged();
    this.LayoutTab.OnCurrentLayoutElementChanged();
    this.LayoutEdit.OnCurrentLayoutElementChanged();
    this.CanChangeProfile = true;
  }

  /// @copydoc Common::GUI::IBindingProfile::OnProfileChanged
  public void OnProfileChanged() {
    this.CanChangeProfile = false;
    this.TargetWindow.OnProfileChanged();
    this.Area.OnProfileChanged();
    this.Options.OnProfileChanged();
    this.ResizeMethod.OnProfileChanged();
    this.LayoutParameter.OnProfileChanged();
    this.LayoutTab.OnProfileChanged();
    this.LayoutEdit.OnProfileChanged();
    this.CanChangeProfile = true;
  }

  //===================================================================
  // コマンドイベントハンドラ
  //===================================================================

  //-------------------------------------------------------------------
  // ApplicationCommands
  //-------------------------------------------------------------------

  /// New
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnNew(object sender, ExecutedRoutedEventArgs e) {
    var result = MessageBox.Show("Do you want to save changes?",
                                 "SCFF.GUI",
                                 MessageBoxButton.YesNoCancel,
                                 MessageBoxImage.Warning,
                                 MessageBoxResult.Yes);
    switch (result) {
      case MessageBoxResult.No: {
        App.Profile.RestoreDefault();
        this.OnProfileChanged();
        break;
      }
      case MessageBoxResult.Yes: {
        var save = new SaveFileDialog();
        save.Title = "SCFF.GUI";
        save.Filter = "SCFF.GUI Profile|*.SCFF.GUI.profile";
        var saveResult = save.ShowDialog();
        if (saveResult.HasValue && (bool)saveResult) {
          /// @todo(me) 実装
          MessageBox.Show(save.FileName);
          App.Options.AddRecentProfile(save.FileName);
          this.MainMenu.OnOptionsChanged();

          App.Profile.RestoreDefault();
          this.OnProfileChanged();
        }
        break;
      }
    }
  }

  /// Open
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnOpen(object sender, ExecutedRoutedEventArgs e) {
    /// @todo(me) Newと似たコードが必要だがかなりめんどくさい。あとでかく
  }

  /// Save
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnSave(object sender, ExecutedRoutedEventArgs e) {
    /// @todo(me) すでに保存されていない場合はダイアログをだす
    var save = new SaveFileDialog();
    save.Title = "SCFF.GUI";
    save.Filter = "SCFF.GUI Profile|*.SCFF.GUI.profile";
    var saveResult = save.ShowDialog();
    if (saveResult.HasValue && (bool)saveResult) {
      /// @todo(me) 実装
      MessageBox.Show(save.FileName);
      App.Options.AddRecentProfile(save.FileName);
      this.MainMenu.OnOptionsChanged();
    }
  }

  /// SaveAs
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnSaveAs(object sender, ExecutedRoutedEventArgs e) {
    var save = new SaveFileDialog();
    save.Title = "SCFF.GUI";
    save.Filter = "SCFF.GUI Profile|*.SCFF.GUI.profile";
    var saveResult = save.ShowDialog();
    if (saveResult.HasValue && (bool)saveResult) {
      /// @todo(me) 実装
      MessageBox.Show(save.FileName);
      App.Options.AddRecentProfile(save.FileName);
      this.MainMenu.OnOptionsChanged();
    }
  }

  //-------------------------------------------------------------------
  // Windows.Shell.SystemCommands
  //-------------------------------------------------------------------
  
  /// CloseWindow
	private void OnCloseWindow(object sender, ExecutedRoutedEventArgs e) {
		SystemCommands.CloseWindow(this);
	}
  /// MaximizeWindow
	private void OnMaximizeWindow(object sender, ExecutedRoutedEventArgs e) {
		SystemCommands.MaximizeWindow(this);
	}
  /// MinimizeWindow
	private void OnMinimizeWindow(object sender, ExecutedRoutedEventArgs e) {
		SystemCommands.MinimizeWindow(this);
	}
  /// RestoreWindow
	private void OnRestoreWindow(object sender, ExecutedRoutedEventArgs e) {
		SystemCommands.RestoreWindow(this);
	}

  //-------------------------------------------------------------------
  // SCFF.GUI.Commands
  //-------------------------------------------------------------------

  /// @copydoc Commands::CurrentLayoutElementVisualChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnCurrentLayoutElementVisualChanged(object sender, ExecutedRoutedEventArgs e) {
    this.LayoutEdit.OnCurrentLayoutElementChanged();
  }
  /// @copydoc Commands::ProfileVisualChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnProfileVisualChanged(object sender, ExecutedRoutedEventArgs e) {
    this.LayoutEdit.OnOptionsChanged();
    // 内部でOnProfileChangedと同じ処理が走る
  }
  /// @copydoc Commands::ProfileStructureChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnProfileStructureChanged(object sender, ExecutedRoutedEventArgs e) {
    // tabの選択を変えないといけないのでEntireじゃなければいけない
    this.OnProfileChanged();
  }
  /// @copydoc Commands::LayoutParameterChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnLayoutParameterChanged(object sender, ExecutedRoutedEventArgs e) {
    this.LayoutParameter.OnCurrentLayoutElementChanged();
  }
  /// @copydoc Commands::TargetWindowChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnTargetWindowChanged(object sender, ExecutedRoutedEventArgs e) {
    this.TargetWindow.OnCurrentLayoutElementChanged();
    // CurrentLayoutElementVisualChanged
    this.LayoutEdit.OnCurrentLayoutElementChanged();
  }
  /// @copydoc Commands::AreaChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnAreaChanged(object sender, ExecutedRoutedEventArgs e) {
    this.Area.OnCurrentLayoutElementChanged();
    // CurrentLayoutElementVisualChanged
    this.LayoutEdit.OnCurrentLayoutElementChanged();
  }
  /// @copydoc Commands::SampleSizeChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnSampleSizeChanged(object sender, ExecutedRoutedEventArgs e) {
    this.LayoutEdit.OnRuntimeOptionsChanged();
    this.LayoutParameter.OnRuntimeOptionsChanged();
  }

  //-------------------------------------------------------------------

  /// @copydoc SetAero
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnSetAero(object sender, ExecutedRoutedEventArgs e) {
    if (!CanUseAero()) return;

    Debug.WriteLine("Execute", "[Command] SetAero");
    this.SetAero();
  }

  /// @copydoc SetCompactView
  /// @param sender 使用しない
  /// @param e 使用しない
  private void OnSetCompactView(object sender, ExecutedRoutedEventArgs e) {
    Debug.WriteLine("Execute", "[Command] SetCompactView");
    this.SetCompactView(true);
  }
}
}   // namespace SCFF.GUI
