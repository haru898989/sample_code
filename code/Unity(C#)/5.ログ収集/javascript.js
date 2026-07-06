function doGet(e) {
  var lock = LockService.getScriptLock();
  try {
    // 10秒間ロックを試みる（他の人の割り込みを禁止する）
    lock.waitLock(10000);
    
    var ss = SpreadsheetApp.getActiveSpreadsheet();
    var configSheet = ss.getSheetByName("Config"); // 設定用シート
    
    // A1セルから現在のIDを取得して+1する
    var currentId = configSheet.getRange("A1").getValue();
    var nextId = parseInt(currentId) + 1;
    
    // 新しいIDをA1に書き戻す
    configSheet.getRange("A1").setValue(nextId);
    
    // ロックを解除して、UnityにIDを返す
    return ContentService.createTextOutput(nextId.toString());
    
  } catch (err) {
    return ContentService.createTextOutput("Error: " + err.message);
  } finally {
    lock.releaseLock();
  }
}


function doPost(e) {
  var lock = LockService.getScriptLock();
  try {
    lock.waitLock(10000);
    var sheet = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
    var data = JSON.parse(e.postData.contents); 
    if (data.length > 0) {
      var lastRow = sheet.getLastRow();
      sheet.getRange(lastRow + 1, 1, data.length, data[0].length).setValues(data);
    }
    return ContentService.createTextOutput("Success");
  } catch (err) {
    return ContentService.createTextOutput("Error: " + err.message);
  } finally {
    lock.releaseLock();
  }
}