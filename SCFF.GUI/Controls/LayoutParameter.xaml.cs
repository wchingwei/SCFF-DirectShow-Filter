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

/// @file SCFF.GUI/Controls/LayoutParameter.xaml.cs
/// @copydoc SCFF::GUI::Controls::LayoutParameter

namespace SCFF.GUI.Controls {

using System;
using System.Diagnostics;
using System.Windows.Controls;
using SCFF.Common;

/// 数値を指定してレイアウト配置を調整するためのUserControl
/// @todo(me) InputValidationが甘すぎるので何とかする
public partial class LayoutParameter
    : UserControl, IUpdateByProfile, IUpdateByRuntimeOptions {
  //===================================================================
  // コンストラクタ/デストラクタ/Closing/ShutdownStartedイベントハンドラ
  //===================================================================

  /// コンストラクタ
  public LayoutParameter() {
    InitializeComponent();
  }

  //===================================================================
  // イベントハンドラ
  //===================================================================

  //-------------------------------------------------------------------
  // *Changed/Checked/Unchecked以外
  //-------------------------------------------------------------------

  /// Fit: Click
  ///
  /// 現在選択中のレイアウト要素のアスペクト比をあわせ、黒帯を消す
  /// @todo(me) コンテキストメニューにも実装したいのでCommand化したい
  ///           (Shiftドラッグで比率維持とかやってもいいかも)
  /// @param sender 使用しない
  /// @param e 使用しない
  private void Fit_Click(object sender, System.Windows.RoutedEventArgs e) {
    if (!App.Profile.CurrentView.IsWindowValid) {
      Debug.WriteLine("Invalid Window", "LayoutParameter.Fit_Click");
      return;
    }

    // Profileの設定を変える
    App.Profile.Current.Open();
    App.Profile.Current.FitBoundRelativeRect(
        App.RuntimeOptions.CurrentSampleWidth,
        App.RuntimeOptions.CurrentSampleHeight);
    App.Profile.Current.Close();

    // 関連するUserControlに更新を伝える
    UpdateCommands.UpdateLayoutEditByCurrentProfile.Execute(null, null);
    // 自分自身はCommandsではなく直接更新する
    this.UpdateByCurrentProfile();
  }

  //-------------------------------------------------------------------
  // Checked/Unchecked
  //-------------------------------------------------------------------

  //-------------------------------------------------------------------
  // *Changed/Collapsed/Expanded
  //-------------------------------------------------------------------

  /// BoundX/BoundY/GetBoundWidth/BoundHeightを更新する
  /// @attention *Changedイベントハンドラが無いのでそのまま代入してOK
  private void UpdateDisabledTextboxes() {
    // dummyの場合もあり
    var sampleWidth = App.RuntimeOptions.CurrentSampleWidth;
    var sampleHeight = App.RuntimeOptions.CurrentSampleHeight;
    var isDummy = App.RuntimeOptions.SelectedEntryIndex == -1;

    this.BoundX.Text =
        App.Profile.CurrentView.GetBoundLeftString(isDummy, sampleWidth);
    this.BoundY.Text =
        App.Profile.CurrentView.GetBoundTopString(isDummy, sampleHeight);
    this.BoundWidth.Text =
        App.Profile.CurrentView.GetBoundWidthString(isDummy, sampleHeight);
    this.BoundHeight.Text =
        App.Profile.CurrentView.GetBoundHeightString(isDummy, sampleWidth);
  }

  /// 下限・上限つきでテキストボックスから値を取得する
  private bool TryParseBoundRelativeParameter(TextBox textBox,
      double lowerBound, double upperBound, out double parsedValue) {
    // Parse
    if (!double.TryParse(textBox.Text, out parsedValue)) {
      parsedValue = lowerBound;
      textBox.Text = lowerBound.ToString("F3");
      return false;
    }

    // Validation
    if (parsedValue < lowerBound) {
      parsedValue = lowerBound;
      textBox.Text = lowerBound.ToString("F3");
      return false;
    } else if (parsedValue > upperBound) {
      parsedValue = upperBound;
      textBox.Text = upperBound.ToString("F3");
      return false;
    }

    return true;
  }

  /// BoundRelativeLeft: TextChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void BoundRelativeLeft_TextChanged(object sender, TextChangedEventArgs e) {
    if (!this.IsEnabledByProfile) return;
    var lowerBound = 0.0;
    var upperBound = 1.0;
    double parsedValue;
    if (this.TryParseBoundRelativeParameter(this.BoundRelativeLeft, lowerBound, upperBound, out parsedValue)) {
      // Profileに書き込み
      App.Profile.Current.Open();
      App.Profile.Current.SetBoundRelativeLeft = parsedValue;
      App.Profile.Current.Close();
      this.UpdateDisabledTextboxes();
      // 関連するコントロールに通知
      UpdateCommands.UpdateLayoutEditByCurrentProfile.Execute(null, null);
    }
  }

  /// BoundRelativeTop: TextChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void BoundRelativeTop_TextChanged(object sender, TextChangedEventArgs e) {
    if (!this.IsEnabledByProfile) return;
    var lowerBound = 0.0;
    var upperBound = 1.0;
    double parsedValue;
    if (this.TryParseBoundRelativeParameter(this.BoundRelativeTop, lowerBound, upperBound, out parsedValue)) {
      // Profileに書き込み
      App.Profile.Current.Open();
      App.Profile.Current.SetBoundRelativeTop = parsedValue;
      App.Profile.Current.Close();
      this.UpdateDisabledTextboxes();
      // 関連するコントロールに通知
      UpdateCommands.UpdateLayoutEditByCurrentProfile.Execute(null, null);
    }
  }

  /// BoundRelativeRight: TextChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void BoundRelativeRight_TextChanged(object sender, TextChangedEventArgs e) {
    if (!this.IsEnabledByProfile) return;
    var lowerBound = 0.0;
    var upperBound = 1.0;
    double parsedValue;
    if (this.TryParseBoundRelativeParameter(this.BoundRelativeRight, lowerBound, upperBound, out parsedValue)) {
      // Profileに書き込み
      App.Profile.Current.Open();
      App.Profile.Current.SetBoundRelativeRight = parsedValue;
      App.Profile.Current.Close();
      this.UpdateDisabledTextboxes();
      // 関連するコントロールに通知
      UpdateCommands.UpdateLayoutEditByCurrentProfile.Execute(null, null);
    }
  }

  /// BoundRelativeBottom: TextChanged
  /// @param sender 使用しない
  /// @param e 使用しない
  private void BoundRelativeBottom_TextChanged(object sender, TextChangedEventArgs e) {
    if (!this.IsEnabledByProfile) return;
    var lowerBound = 0.0;
    var upperBound = 1.0;
    double parsedValue;
    if (this.TryParseBoundRelativeParameter(this.BoundRelativeBottom, lowerBound, upperBound, out parsedValue)) {
      // Profileに書き込み
      App.Profile.Current.Open();
      App.Profile.Current.SetBoundRelativeBottom = parsedValue;
      App.Profile.Current.Close();
      this.UpdateDisabledTextboxes();
      // 関連するコントロールに通知
      UpdateCommands.UpdateLayoutEditByCurrentProfile.Execute(null, null);
    }
  }

  //===================================================================
  // IUpdateByRuntimeOptionsの実装
  //===================================================================

  /// @copydoc IUpdateByRuntimeOptions::IsEnabledByRuntimeOptions
  public bool IsEnabledByRuntimeOptions { get; private set; }
  /// @copydoc IUpdateByRuntimeOptions::UpdateByRuntimeOptions
  public void UpdateByRuntimeOptions() {
    this.IsEnabledByRuntimeOptions = false;
    this.UpdateDisabledTextboxes();
    this.IsEnabledByRuntimeOptions = true;
  }

  //===================================================================
  // IUpdateByProfileの実装
  //===================================================================

  /// GroupBox.Headerの最大文字数
  private const int maxHeaderLength = 60;

  /// @copydoc IUpdateByProfile::IsEnabledByProfile
  public bool IsEnabledByProfile { get; private set; }
  /// @copydoc IUpdateByProfile::UpdateByCurrentProfile
  public void UpdateByCurrentProfile() {
    this.IsEnabledByProfile = false;

    this.GroupBox.Header =
        App.Profile.CurrentView.GetHeaderString(LayoutParameter.maxHeaderLength);

    this.UpdateDisabledTextboxes();

    var isComplexLayout = App.Profile.LayoutType == LayoutTypes.ComplexLayout;
    this.BoundRelativeLeft.IsEnabled = isComplexLayout;
    this.BoundRelativeTop.IsEnabled = isComplexLayout;
    this.BoundRelativeRight.IsEnabled = isComplexLayout;
    this.BoundRelativeBottom.IsEnabled = isComplexLayout;

    // *Changed/Collapsed/Expanded
    this.BoundRelativeLeft.Text = App.Profile.CurrentView.BoundRelativeLeftString;
    this.BoundRelativeTop.Text = App.Profile.CurrentView.BoundRelativeTopString;
    this.BoundRelativeRight.Text = App.Profile.CurrentView.BoundRelativeRightString;
    this.BoundRelativeBottom.Text = App.Profile.CurrentView.BoundRelativeBottomString;

    this.IsEnabledByProfile = true;
  }
  /// @copydoc IUpdateByProfile::UpdateByEntireProfile
  public void UpdateByEntireProfile() {
    // 編集するのはCurrentのみ
    this.UpdateByCurrentProfile();
  }
}
}   // namespace SCFF.GUI.Controls
