export function IndexOfNonRepeat(
  arr: string,
  item: string,
  exp: RegExp,
): { start: number; end: number } {
  for (let start = 0; start < arr.length; start++) {
    if (arr[start] === item) {
      let end = start + 1;
      for (; arr.length !== end && arr[end].match(exp); end++) {}
      if (start + 1 !== end) {
        return { start, end };
      }
    }
  }
  return undefined;
}

export function formatNumber(value: number, toComa?: boolean): string {
  const locale = value.toLocaleString();
  const index = locale.indexOf(',');
  const log = Math.floor(Math.log10(value) / 3);
  return (
    locale.slice(0, index === -1 ? 3 : toComa ? index : index + 2) +
    (log === 1 ? 'K' : log === 2 ? 'M' : '')
  );
}
