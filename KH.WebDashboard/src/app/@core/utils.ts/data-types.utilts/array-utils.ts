export function getItemsByArrayNameKeyword(data: any, keyword: any) {
  const arrayKey = Object.keys(data).find((key) =>
    key.toLowerCase().includes(keyword.toLowerCase())
  );

  if (!arrayKey) {
    return 'No matching array found';
  }

  return data[arrayKey];
}
