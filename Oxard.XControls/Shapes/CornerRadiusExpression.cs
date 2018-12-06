using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Oxard.XControls.Shapes
{
    public class CornerRadiusExpression
    {
        public CornerRadiusExpression(string data)
        {
            this.Split(data);
        }

        public CornerRadius TopLeft { get; private set; }

        public CornerRadius TopRight { get; private set; }

        public CornerRadius BottomRight { get; private set; }

        public CornerRadius BottomLeft { get; private set; }

        private void Split(string data)
        {
            try
            {
                var patterns = data?.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (patterns == null || patterns.Count == 0)
                {
                    this.TopRight = null;
                    this.TopLeft = null;
                    this.BottomLeft = null;
                    this.BottomRight = null;
                    return;
                }

                double tlX = 0, tlY = 0, trX = 0, trY = 0, brX = 0, brY = 0, blX = 0, blY = 0;

                List<Match> matches = new List<Match>();

                foreach (var pattern in patterns)
                {
                    var match = Regex.Match(
                        pattern,
                        "^(?<name>tr|tl|br|bl)?(?<number>\\d+(\\.\\d+)?)$",
                        RegexOptions.IgnoreCase);
                    if (!match.Success) throw this.CreateNotSupportedCornerRadiusException();

                    matches.Add(match);
                }

                if (matches[0].Groups["name"].Success)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var currentMatch = matches[i];
                        var nextMatch = i + 1 < matches.Count ? matches[i + 1] : null;

                        if (!currentMatch.Groups["name"].Success) throw this.CreateNotSupportedCornerRadiusException();

                        Tuple<double, double, bool> valuesForCorner;
                        switch (currentMatch.Groups["name"].Value.ToLower())
                        {
                            case "tl":
                                valuesForCorner = this.GetXyFromMatches(currentMatch, nextMatch);
                                tlX = valuesForCorner.Item1;
                                tlY = valuesForCorner.Item2;
                                break;
                            case "tr":
                                valuesForCorner = this.GetXyFromMatches(currentMatch, nextMatch);
                                trX = valuesForCorner.Item1;
                                trY = valuesForCorner.Item2;
                                break;
                            case "bl":
                                valuesForCorner = this.GetXyFromMatches(currentMatch, nextMatch);
                                blX = valuesForCorner.Item1;
                                blY = valuesForCorner.Item2;
                                break;
                            case "br":
                                valuesForCorner = this.GetXyFromMatches(currentMatch, nextMatch);
                                brX = valuesForCorner.Item1;
                                brY = valuesForCorner.Item2;
                                break;
                            default: throw this.CreateNotSupportedCornerRadiusException();
                        }

                        if (valuesForCorner.Item3) i++;
                    }
                }
                else
                {
                    try
                    {
                        if (matches.Count == 1)
                            tlX = tlY = trX = trY = brX = brY = blX = blY = Convert.ToDouble(
                                                                          matches[0].Groups["number"].Value);
                        else if (matches.Count == 2)
                        {
                            tlX = trX = brX = blX = Convert.ToDouble(matches[0].Groups["number"].Value);
                            tlY = trY = brY = blY = Convert.ToDouble(matches[1].Groups["number"].Value);
                        }
                        else if (matches.Count == 4)
                        {
                            tlX = tlY = Convert.ToDouble(matches[0].Groups["number"].Value);
                            trX = trY = Convert.ToDouble(matches[1].Groups["number"].Value);
                            brX = brY = Convert.ToDouble(matches[2].Groups["number"].Value);
                            blX = blY = Convert.ToDouble(matches[3].Groups["number"].Value);
                        }
                        else if (matches.Count == 8)
                        {
                            tlX = Convert.ToDouble(matches[0].Groups["number"].Value);
                            trX = Convert.ToDouble(matches[2].Groups["number"].Value);
                            brX = Convert.ToDouble(matches[4].Groups["number"].Value);
                            blX = Convert.ToDouble(matches[6].Groups["number"].Value);
                            tlY = Convert.ToDouble(matches[1].Groups["number"].Value);
                            trY = Convert.ToDouble(matches[3].Groups["number"].Value);
                            brY = Convert.ToDouble(matches[5].Groups["number"].Value);
                            blY = Convert.ToDouble(matches[7].Groups["number"].Value);
                        }
                        else throw this.CreateNotSupportedCornerRadiusException();
                    }
                    catch (Exception e)
                    {
                        if (e is NotSupportedException) throw;
                        throw this.CreateNotSupportedCornerRadiusException(e);
                    }
                }

                this.TopLeft = new CornerRadius(tlX, tlY);
                this.TopRight = new CornerRadius(trX, trY);
                this.BottomLeft = new CornerRadius(blX, blY);
                this.BottomRight = new CornerRadius(brX, brY);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CornerRadiusExpression format error : {ex.Message}");
            }
        }

        private Tuple<double, double, bool> GetXyFromMatches(Match currentMatch, Match nextMatch)
        {
            var nextIsOther = (nextMatch?.Groups["name"]?.Success).GetValueOrDefault();

            try
            {
                var x = Convert.ToDouble(currentMatch.Groups["number"].Value);
                double y;
                if (!nextIsOther && nextMatch != null)
                {
                    y = Convert.ToDouble(nextMatch.Groups["number"].Value);
                    return new Tuple<double, double, bool>(x, y, true);
                }
                else return new Tuple<double, double, bool>(x, x, false);
            }
            catch (Exception e)
            {
                throw this.CreateNotSupportedCornerRadiusException(e);
            }
        }

        private Exception CreateNotSupportedCornerRadiusException(Exception innerException = null)
        {
            return new NotSupportedException("Expression for CornerRadius was not formatted correctly. It must be as : n or x,y ou TRx,y BRx,y ou x1,y1,x2,y2,x3,y3,x4,y4 ...", innerException);
        }
    }
}
