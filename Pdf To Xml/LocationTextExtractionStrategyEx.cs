﻿using iTextSharp.text.pdf.parser;
using System.Text;

namespace Pdf_To_Xml
{
    class LocationTextExtractionStrategyEx : LocationTextExtractionStrategy
    {
        private List<TextChunk> m_locationResult = new List<TextChunk>();
        private List<TextInfo> m_TextLocationInfo = new List<TextInfo>();
        private List<List<TextChunk>> m_CharacterLocationInfos = new List<List<TextChunk>>();
        private List<List<TextInfo>> m_CharacterInfos = new List<List<TextInfo>>();

        public List<TextChunk> LocationResult
        {
            get { return m_locationResult; }
        }
        public List<TextInfo> TextLocationInfo
        {
            get { return m_TextLocationInfo; }
        }

        public List<List<TextChunk>> TextCharacterLocationInfos
        {
            get { return m_CharacterLocationInfos; }
        }

        public List<List<TextInfo>> TextCharacterInfos
        {
            get { return m_CharacterInfos; }
        }

        /// <summary>
        /// Creates a new LocationTextExtracationStrategyEx
        /// </summary>
        public LocationTextExtractionStrategyEx()
        {
        }

        /// <summary>
        /// Returns the result so far
        /// </summary>
        /// <returns>a String with the resulting text</returns>
        public override String GetResultantText()
        {
            m_locationResult.Sort();

            StringBuilder sb = new StringBuilder();
            TextChunk lastChunk = null;
            TextInfo lastTextInfo = null;
            //List<TextInfo> lastTextInfoList = new List<TextInfo>();

            for (int i = 0; i < m_locationResult.Count; i++)
            {
                if (lastChunk == null)
                {
                    sb.Append(m_locationResult[i].Text);
                    lastTextInfo = new TextInfo(m_locationResult[i]);

                    //lastTextInfoList = new List<TextInfo>();
                    /*foreach (var s in m_CharacterLocationInfos[i])
                    {
                        lastTextInfoList.Add(new TextInfo(s));
                    }
*/
                    m_TextLocationInfo.Add(lastTextInfo);
                   // m_CharacterInfos.Add(lastTextInfoList);
                }
                else
                {
                    if (m_locationResult[i].sameLine(lastChunk))
                    {
                        float dist = m_locationResult[i].distanceFromEndOf(lastChunk);

                        if (dist < -m_locationResult[i].CharSpaceWidth)
                        {
                            sb.Append(' ');
                            lastTextInfo.addSpace();
                           // lastTextInfoList[lastTextInfoList.Count - 1].addSpace();
                        }
                        //append a space if the trailing char of the prev string wasn't a space && the 1st char of the current string isn't a space
                        else if (dist > m_locationResult[i].CharSpaceWidth / 2.0f && m_locationResult[i].Text[0] != ' ' && lastChunk.Text[lastChunk.Text.Length - 1] != ' ')
                        {
                            sb.Append(' ');
                            lastTextInfo.addSpace();
                            //lastTextInfoList[lastTextInfoList.Count - 1].addSpace();
                        }
                        sb.Append(m_locationResult[i].Text);
                        lastTextInfo.appendText(m_locationResult[i]);

                       /* foreach (var s in m_CharacterLocationInfos[i])
                        {
                            lastTextInfoList.Add(new TextInfo(s));
                        }*/

                    }
                    else
                    {
                        sb.Append('\n');
                        sb.Append(m_locationResult[i].Text);
                        lastTextInfo = new TextInfo(m_locationResult[i]);

                        /*lastTextInfoList = new List<TextInfo>();
                        foreach (var s in m_CharacterLocationInfos[i])
                        {
                            lastTextInfoList.Add(new TextInfo(s));
                        }*/

                        m_TextLocationInfo.Add(lastTextInfo);
                        //m_CharacterInfos.Add(lastTextInfoList);
                    }
                }
                lastChunk = m_locationResult[i];
            }
            return sb.ToString();
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderInfo"></param>
        public override void RenderText(TextRenderInfo renderInfo)
        {
            LineSegment segment = renderInfo.GetBaseline();
            TextChunk location = new TextChunk(renderInfo.GetText(), segment.GetStartPoint(), segment.GetEndPoint(), renderInfo.GetSingleSpaceWidth(), renderInfo.GetAscentLine(), renderInfo.GetDescentLine());
            /*foreach(var text in renderInfo.GetCharacterRenderInfos())
            {
                LineSegment segment1 = text.GetBaseline();
                TextChunk location1 = new TextChunk(text.GetText(), segment1.GetStartPoint(), segment1.GetEndPoint(), text.GetSingleSpaceWidth(), text.GetAscentLine(), text.GetDescentLine());
                chunks.Add(location1);
            }*/
            m_locationResult.Add(location);
           /* chunks.Sort();
            m_CharacterLocationInfos.Add(chunks);*/
        }

        public class TextChunk : IComparable, ICloneable
        {
            string m_text;
            Vector m_startLocation;
            Vector m_endLocation;
            Vector m_orientationVector;
            int m_orientationMagnitude;
            int m_distPerpendicular;
            float m_distParallelStart;
            float m_distParallelEnd;
            float m_charSpaceWidth;

            public LineSegment AscentLine;
            public LineSegment DecentLine;

            public object Clone()
            {
                TextChunk copy = new TextChunk(m_text, m_startLocation, m_endLocation, m_charSpaceWidth, AscentLine, DecentLine);
                return copy;
            }

            public string Text
            {
                get { return m_text; }
                set { m_text = value; }
            }
            public float CharSpaceWidth
            {
                get { return m_charSpaceWidth; }
                set { m_charSpaceWidth = value; }
            }
            public Vector StartLocation
            {
                get { return m_startLocation; }
                set { m_startLocation = value; }
            }
            public Vector EndLocation
            {
                get { return m_endLocation; }
                set { m_endLocation = value; }
            }

            /// <summary>
            /// Represents a chunk of text, it's orientation, and location relative to the orientation vector
            /// </summary>
            /// <param name="txt"></param>
            /// <param name="startLoc"></param>
            /// <param name="endLoc"></param>
            /// <param name="charSpaceWidth"></param>
            public TextChunk(string txt, Vector startLoc, Vector endLoc, float charSpaceWidth, LineSegment ascentLine, LineSegment decentLine)
            {
                m_text = txt;
                m_startLocation = startLoc;
                m_endLocation = endLoc;
                m_charSpaceWidth = charSpaceWidth;
                AscentLine = ascentLine;
                DecentLine = decentLine;

                m_orientationVector = m_endLocation.Subtract(m_startLocation).Normalize();
                m_orientationMagnitude = (int)(Math.Atan2(m_orientationVector[Vector.I2], m_orientationVector[Vector.I1]) * 1000);

                // see http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
                // the two vectors we are crossing are in the same plane, so the result will be purely
                // in the z-axis (out of plane) direction, so we just take the I3 component of the result
                Vector origin = new Vector(0, 0, 1);
                m_distPerpendicular = (int)(m_startLocation.Subtract(origin)).Cross(m_orientationVector)[Vector.I3];

                m_distParallelStart = m_orientationVector.Dot(m_startLocation);
                m_distParallelEnd = m_orientationVector.Dot(m_endLocation);
            }

            /// <summary>
            /// true if this location is on the the same line as the other text chunk
            /// </summary>
            /// <param name="textChunkToCompare">the location to compare to</param>
            /// <returns>true if this location is on the the same line as the other</returns>
            public bool sameLine(TextChunk textChunkToCompare)
            {
                if (m_orientationMagnitude != textChunkToCompare.m_orientationMagnitude) return false;
                if (m_distPerpendicular != textChunkToCompare.m_distPerpendicular) return false;
                return true;
            }

            /// <summary>
            /// Computes the distance between the end of 'other' and the beginning of this chunk
            /// in the direction of this chunk's orientation vector.  Note that it's a bad idea
            /// to call this for chunks that aren't on the same line and orientation, but we don't
            /// explicitly check for that condition for performance reasons.
            /// </summary>
            /// <param name="other"></param>
            /// <returns>the number of spaces between the end of 'other' and the beginning of this chunk</returns>
            public float distanceFromEndOf(TextChunk other)
            {
                float distance = m_distParallelStart - other.m_distParallelEnd;
                return distance;
            }

            /// <summary>
            /// Compares based on orientation, perpendicular distance, then parallel distance
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                if (obj == null) throw new ArgumentException("Object is now a TextChunk");

                TextChunk rhs = obj as TextChunk;
                if (rhs != null)
                {
                    if (this == rhs) return 0;

                    int rslt;
                    rslt = m_orientationMagnitude - rhs.m_orientationMagnitude;
                    if (rslt != 0) return rslt;

                    rslt = m_distPerpendicular - rhs.m_distPerpendicular;
                    if (rslt != 0) return rslt;

                    // note: it's never safe to check floating point numbers for equality, and if two chunks
                    // are truly right on top of each other, which one comes first or second just doesn't matter
                    // so we arbitrarily choose this way.
                    rslt = m_distParallelStart < rhs.m_distParallelStart ? -1 : 1;

                    return rslt;
                }
                else
                {
                    throw new ArgumentException("Object is now a TextChunk");
                }
            }
        }

        public class TextInfo
        {
            public Vector TopLeft;
            public Vector BottomRight;
            private string m_Text;

            public string Text
            {
                get { return m_Text; }
            }

            /// <summary>
            /// Create a TextInfo.
            /// </summary>
            /// <param name="initialTextChunk"></param>
            public TextInfo(TextChunk initialTextChunk)
            {
                TopLeft = initialTextChunk.AscentLine.GetStartPoint();
                BottomRight = initialTextChunk.DecentLine.GetEndPoint();
                m_Text = initialTextChunk.Text;
            }

            /// <summary>
            /// Add more text to this TextInfo.
            /// </summary>
            /// <param name="additionalTextChunk"></param>
            public void appendText(TextChunk additionalTextChunk)
            {
                BottomRight = additionalTextChunk.DecentLine.GetEndPoint();
                m_Text += additionalTextChunk.Text;
            }

            /// <summary>
            /// Add a space to the TextInfo.  This will leave the endpoint out of sync with the text.
            /// The assumtion is that you will add more text after the space which will correct the endpoint.
            /// </summary>
            public void addSpace()
            {
                m_Text += ' ';
            }


        }
    }
}
